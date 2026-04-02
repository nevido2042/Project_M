using UnityEngine;
using UnityEngine.Pool;

namespace Hero
{
    /// <summary>
    /// 오브젝트 풀링을 관리하는 컴포넌트 (GameManager를 통해 접근)
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        [Header("몬스터 풀 설정")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject expItemPrefab;
        [SerializeField] private int defaultCapacity = 20;
        [SerializeField] private int maxPoolSize = 100;

        private IObjectPool<Enemy> enemyPool;
        private IObjectPool<ExperienceItem> expPool;

        private void Awake()
        {
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
            enemy.SetPool(enemyPool); 
            return enemy;
        }

        private void OnGetEnemy(Enemy enemy) => enemy.gameObject.SetActive(true);
        private void OnReleaseEnemy(Enemy enemy) => enemy.gameObject.SetActive(false);
        private void OnDestroyEnemy(Enemy enemy) => Destroy(enemy.gameObject);

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

        public Enemy GetEnemy() => enemyPool.Get();
        public ExperienceItem GetExperienceItem() => expPool.Get();
    }
}
