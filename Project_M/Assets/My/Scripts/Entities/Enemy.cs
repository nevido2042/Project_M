using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 몬스터의 전체적인 행동을 관리하는 클래스
    /// </summary>
    public class Enemy : MonoBehaviour, IRepositionable, IDamageable
    {
        

        [Header("데이터 설정")]
        [SerializeField] private EnemyData data; // 공통 능력치 데이터 에셋
        private float currentHealth;
        private Move move;         // 이동 컴포넌트 참조
        private UnityEngine.Pool.IObjectPool<Enemy> pool; // 자신을 관리하는 풀 참조

        // 인터페이스 구현: 체력 및 무적 정보
        public float CurrentHealth => currentHealth;
        public float MaxHealth => data != null ? data.MaxHealth : 0f;
        public bool IsInvincible => false;

        private void Awake()
        {
            move = GetComponent<Move>();

            // 초기 체력 및 속도 설정
            if (data != null)
            {
                currentHealth = data.MaxHealth;
                if (move != null) move.Speed = data.MoveSpeed;
            }
        }

        private void OnEnable()
        {
            // 풀에서 꺼내질 때 체력 및 속도 초기화
            if (data != null)
            {
                currentHealth = data.MaxHealth;
                if (move != null) move.Speed = data.MoveSpeed;
            }
        }

        /// <summary>
        /// 자신이 속한 풀을 설정합니다 (PoolManager에서 호출)
        /// </summary>
        public void SetPool(UnityEngine.Pool.IObjectPool<Enemy> pool)
        {
            this.pool = pool;
        }

        private void Start()
        {
            // 이제 플레이어 정보 캐싱은 Reposition 컴포넌트에서 통합 관리합니다.
        }

        /// <summary>
        /// 플레이어의 위치와 진행 방향을 계산하여 적을 앞쪽 구역에 재배치합니다.
        /// </summary>
        public void Reposition(Vector3 playerPos, Vector2 playerDir)
        {
            // 재배치 거리 및 랜덤 오프셋 계산
            float range = 20f;
            Vector3 spawnPos = playerPos + (Vector3)(playerDir.normalized * range);
            spawnPos += new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);

            // 새로운 위치로 순간이동
            transform.position = spawnPos;

            // 재배치될 때 체력도 다시 채워줌 (재활용할 경우)
            if (data != null) currentHealth = data.MaxHealth;
        }

        /// <summary>
        /// 몬스터에게 데미지를 입힙니다.
        /// </summary>
        /// <param name="damage">입힐 데미지 양</param>
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            // 몬스터 사망 시 경험치 아이템 생성
            ExperienceItem expItem = PoolManager.Instance.GetExperienceItem();
            if (expItem != null)
            {
                expItem.transform.position = transform.position;
            }

            // 풀 시스템을 사용할 경우 풀로 반납, 아니면 비활성화
            if (pool != null)
            {
                pool.Release(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        
        /// </summary>


        /// <summary>
        /// 플레이어와 접촉 중일 때 데미지를 입힘
        /// </summary>
        private void OnCollisionStay2D(Collision2D collision)
        {
            // 충돌 대상이 플레이어 태그를 가지고 있다면
            if (collision.gameObject.CompareTag("Player"))
            {
                // IDamageable 인터페이스를 찾아 데미지를 줍니다.
                // 무적 여부 등은 플레이어 스크립트 내부에서 처리됩니다.
                IDamageable hit = collision.gameObject.GetComponent<IDamageable>();
                if (hit != null && data != null)
                {
                    hit.TakeDamage(data.DamageAmount);
                }
            }
        }
    }
}
