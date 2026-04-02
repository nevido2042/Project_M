using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 모든 핵심 시스템을 중앙에서 관리하는 퍼사드(Facade) 매니저
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
                    // 씬에서 인스턴스 찾기
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

        // 외부에서 접근할 수 있는 읽기 전용 프로퍼티
        public PoolManager Pool => pool;
        public EnemySpawner Spawner => spawner;
        public UpgradeManager Upgrade => upgrade;
        public Player Player => player;

        private void Awake()
        {
            // 싱글톤 중복 방지
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            
            // 필요 시 씬 전환 후에도 유지
            // DontDestroyOnLoad(gameObject);
        }
    }
}
