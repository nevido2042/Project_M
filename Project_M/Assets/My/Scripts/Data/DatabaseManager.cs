using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// Manager class for MSSQL database integration.
    /// Supports cross-device tracking via SQL Authentication and async operations.
    /// </summary>
    public class DatabaseManager : MonoBehaviour
    {
        [Header("MSSQL Connection Settings")]
        [SerializeField] private string serverAddress = @"localhost\SQLEXPRESS"; // Default to localhost for better reliability
        [SerializeField] private string databaseName = "master";
        [SerializeField] private string userId = "sa";
        [SerializeField] private string password = "";
        [SerializeField] private bool useWindowsAuthentication = false; // Default to false for cross-device support
        [SerializeField] private int timeout = 10;

        [Header("UI Reference")]
        [SerializeField] private TextMeshProUGUI lastPlayTimeText;

        private string ConnectionString
        {
            get
            {
                // SQL Express usually requires TrustServerCertificate for modern versions
                string options = $"Connect Timeout={timeout};TrustServerCertificate=True;";
                
                if (useWindowsAuthentication)
                {
                    return $"Server={serverAddress};Database={databaseName};Integrated Security=True;{options}";
                }
                return $"Server={serverAddress};Database={databaseName};User ID={userId};Password={password};{options}";
            }
        }

        private async void Start()
        {
            await CheckAndCreateSchemaAsync();
            await UpdateLastPlayTimeUIAsync();
        }

        /// <summary>
        /// Ensures the necessary table exists.
        /// </summary>
        private async Task CheckAndCreateSchemaAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    string query = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PlayStats' AND xtype='U')
                        CREATE TABLE PlayStats (ID INT PRIMARY KEY, LastPlayTime DATETIME)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Database] Schema check failed: {e.Message}");
            }
        }

        public async Task UpdateLastPlayTimeUIAsync()
        {
            if (lastPlayTimeText == null) return;

            lastPlayTimeText.text = "Loading...";
            DateTime? lastTime = await GetLastPlayTimeAsync();
            
            if (lastTime.HasValue)
            {
                lastPlayTimeText.text = $"Last Played: {lastTime.Value:yyyy-MM-dd HH:mm}";
            }
            else
            {
                lastPlayTimeText.text = "No previous records found.";
            }
        }

        public async Task SaveLastPlayTimeAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    string query = @"
                        IF EXISTS (SELECT 1 FROM PlayStats WHERE ID = 1)
                            UPDATE PlayStats SET LastPlayTime = GETDATE() WHERE ID = 1
                        ELSE
                            INSERT INTO PlayStats (ID, LastPlayTime) VALUES (1, GETDATE())";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
                Debug.Log("[Database] Last play time saved successfully.");
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex, "Save");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Database] Save failed: {e.Message}");
            }
        }

        public async Task<DateTime?> GetLastPlayTimeAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT LastPlayTime FROM PlayStats WHERE ID = 1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        object result = await command.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            return (DateTime)result;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex, "Load");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Database] Load failed: {e.Message}");
            }
            return null;
        }

        private void HandleSqlException(SqlException ex, string operation)
        {
            Debug.LogError($"[Database] {operation} SQL Error ({ex.Number}): {ex.Message}");
            
            // Common specialized error hints
            if (ex.Number == 10054)
            {
                Debug.LogError("[Database] Hint: Connection reset. Check if TCP/IP is enabled and SQL Browser is running.");
            }
            else if (ex.Number == 18456)
            {
                Debug.LogError("[Database] Hint: Login failed. Check User ID and Password.");
            }
            else if (ex.Number == 2 || ex.Number == 53)
            {
                Debug.LogError("[Database] Hint: Server not found. Check Server Address and Port 1433 in firewall.");
            }
        }

        // Legacy support wrappers if needed
        [Obsolete("Use GetLastPlayTimeAsync instead")]
        public DateTime? GetLastPlayTime() => GetLastPlayTimeAsync().GetAwaiter().GetResult();
        
        [Obsolete("Use SaveLastPlayTimeAsync instead")]
        public void SaveLastPlayTime() => SaveLastPlayTimeAsync().Wait();
    }
}
