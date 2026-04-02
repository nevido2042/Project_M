using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hero
{
    /// <summary>
    /// 모든 핵심 시스템과 게임의 흐름(시작/종료)을 중앙에서 관리하는 퍼사드(Facade) 매니저
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Object.FindAnyObjectByType<GameManager>();
                    if (instance == null)
                    {
                        Debug.LogError("씬에 GameManager가 존재하지 않습니다!");
                    }
                }
                return instance;
            }
        }

        [Header("핵심 시스템 참조")]
        [SerializeField] private PoolManager pool;
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private UpgradeManager upgrade;
        [SerializeField] private Player player;

        [Header("UI 시스템 참조")]
        [SerializeField] private GameObject menuUI;      // 로비/메인 메뉴
        [SerializeField] private GameObject pauseMenuUI; // 일시 정지 메뉴
        [SerializeField] private GameObject pauseButton; // 인게임 일시 정지 버튼 (HUD)

        // 외부에서 접근할 수 있는 읽기 전용 프로퍼티
        public PoolManager Pool => pool;
        public EnemySpawner Spawner => spawner;
        public UpgradeManager Upgrade => upgrade;
        public Player Player => player;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void Start()
        {
            // [초기 상태: 로비]
            Time.timeScale = 0f;
            
            if (menuUI != null) menuUI.SetActive(true);
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false); // 메뉴는 숨김
            if (pauseButton != null) pauseButton.SetActive(false); // 정지 버튼도 숨김
        }

        /// <summary>
        /// 실제 게임을 기동 (로비 -> 인게임 전환 시 호출)
        /// </summary>
        public void StartGame()
        {
            Time.timeScale = 1f;
            
            if (menuUI != null) menuUI.SetActive(false);
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
            if (pauseButton != null) pauseButton.SetActive(true); // 게임 시작 후에만 보임

            Debug.Log("[Game] 몬스터 정벌이 시작되었습니다!");
        }

        /// <summary>
        /// 게임 일시 정지 (플레이 중 호출)
        /// </summary>
        public void PauseGame()
        {
            Time.timeScale = 0f;
            
            if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
            if (pauseButton != null) pauseButton.SetActive(false); // 정지 중에는 숨김 (선택 사항)

            Debug.Log("[Game] 게임이 일시 정지되었습니다.");
        }

        /// <summary>
        /// 게임 재개 (PauseMenuUI에서 호출 권장)
        /// </summary>
        public void ResumeGame(GameObject targetPauseMenu)
        {
            Time.timeScale = 1f;
            
            if (targetPauseMenu != null) targetPauseMenu.SetActive(false);
            if (pauseButton != null) pauseButton.SetActive(true); // 다시 보이게 설정

            Debug.Log("[Game] 전투를 재개합니다.");
        }

        /// <summary>
        /// 씬 리스타트
        /// </summary>
        public void RestartScene()
        {
            Time.timeScale = 1f;
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        /// <summary>
        /// 종료
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
