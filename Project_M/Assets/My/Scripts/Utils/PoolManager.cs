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

        [Header("기타 풀 설정")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject gunItemPrefab;

        [SerializeField] private int defaultCapacity = 20;
        [SerializeField] private int maxPoolSize = 100;

        private IObjectPool<Enemy> enemyPool;
        private IObjectPool<ExperienceItem> expPool;
        private IObjectPool<Bullet> bulletPool;

        private void Awake()
        {
            // 유니티 공식 ObjectPool 초기화
            enemyPool = new ObjectPool<Enemy>(
                CreateEnemy, OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy,
                true, defaultCapacity, maxPoolSize
            );

            // 경험치 아이템 풀 초기화
            expPool = new ObjectPool<ExperienceItem>(
                CreateExpItem, OnGetExpItem, OnReleaseExpItem, OnDestroyExpItem,
                true, defaultCapacity, maxPoolSize
            );

            // 탄환 풀 초기화
            bulletPool = new ObjectPool<Bullet>(
                CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet,
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

        private Bullet CreateBullet()
        {
            GameObject obj = Instantiate(bulletPrefab, transform);
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.SetPool(bulletPool);
            return bullet;
        }

        private void OnGetBullet(Bullet bullet) => bullet.gameObject.SetActive(true);
        private void OnReleaseBullet(Bullet bullet) => bullet.gameObject.SetActive(false);
        private void OnDestroyBullet(Bullet bullet) => Destroy(bullet.gameObject);


        #endregion

        public Enemy GetEnemy() => enemyPool.Get();
        public ExperienceItem GetExperienceItem() => expPool.Get();
        public Bullet GetBullet() => bulletPool.Get();
    }
}
