using UnityEngine;
using UnityEngine.Pool;

namespace Hero
{
    /// <summary>
    /// 오브젝트 풀링을 총괄 관리하는 싱글톤 클래스
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        [Header("몬스터 풀 설정")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject expItemPrefab; // 경험치 아이템 프리팹
        [SerializeField] private int defaultCapacity = 20;
        [SerializeField] private int maxPoolSize = 100;

        private IObjectPool<Enemy> enemyPool;
        private IObjectPool<ExperienceItem> expPool; // 경험치 아이템 풀

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            // 유니티 공식 ObjectPool 초기화
            enemyPool = new ObjectPool<Enemy>(
                CreateEnemy,       // 풀에 오브젝트가 없을 때 생성하는 함수
                OnGetEnemy,        // 풀에서 꺼낼 때 실행할 함수
                OnReleaseEnemy,    // 풀에 반납할 때 실행할 함수
                OnDestroyEnemy,    // 풀이 가득 찼을 때 삭제하는 함수
                true,              // 컬렉션 체크 여부
                defaultCapacity,   // 초기 용량
                maxPoolSize        // 최대 용량
            );

            // 경험치 아이템 풀 초기화
            expPool = new ObjectPool<ExperienceItem>(
                CreateExpItem, OnGetExpItem, OnReleaseExpItem, OnDestroyExpItem,
                true, defaultCapacity, maxPoolSize
            );
        }

        #region Pool Callbacks
        private Enemy CreateEnemy()
        {
            GameObject obj = Instantiate(enemyPrefab, transform);
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.SetPool(enemyPool); // Enemy에게 자신이 속한 풀 정보를 전달
            return enemy;
        }

        private void OnGetEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        private void OnReleaseEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        private void OnDestroyEnemy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }

        private ExperienceItem CreateExpItem()
        {
            GameObject obj = Instantiate(expItemPrefab, transform);
            ExperienceItem item = obj.GetComponent<ExperienceItem>();
            item.SetPool(expPool);
            return item;
        }

        private void OnGetExpItem(ExperienceItem item) => item.gameObject.SetActive(true);
        private void OnReleaseExpItem(ExperienceItem item) => item.gameObject.SetActive(false);
        private void OnDestroyExpItem(ExperienceItem item) => Destroy(item.gameObject);
        #endregion

        /// <summary>
        /// 풀에서 적 오브젝트를 하나 가져옵니다.
        /// </summary>
        public Enemy GetEnemy() => enemyPool.Get();

        /// <summary>
        /// 풀에서 경험치 아이템을 하나 가져옵니다.
        /// </summary>
        public ExperienceItem GetExperienceItem() => expPool.Get();
    }
}
