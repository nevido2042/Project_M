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
        private IKnockbackable knockbackable; // 넉백 인터페이스 추가
        private DamageFlash damageFlash;      // 데미지 깜빡임 추가
        private UnityEngine.Pool.IObjectPool<Enemy> pool; // 자신을 관리하는 풀 참조

        // 인터페이스 구현: 체력 및 무적 정보
        public float CurrentHealth => currentHealth;
        public float MaxHealth => data != null ? data.MaxHealth : 0f;
        public bool IsInvincible => false;

        private void Awake()
        {
            move = GetComponent<Move>();
            knockbackable = GetComponent<IKnockbackable>();
            damageFlash = GetComponent<DamageFlash>();

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
        /// <param name="damageSourcePos">데미지를 준 원점 (넉백 방향 계산용)</param>
        public void TakeDamage(float damage, Vector2? damageSourcePos = null)
        {
            currentHealth -= damage;

            // 깜빡임 효과 실행
            if (damageFlash != null) damageFlash.CallFlash();

            // 넉백 적용
            if (knockbackable != null)
            {
                Vector2 dir;
                if (damageSourcePos.HasValue)
                {
                    dir = ((Vector2)transform.position - damageSourcePos.Value).normalized;
                }
                else if (GameManager.Instance != null && GameManager.Instance.Player != null)
                {
                    dir = (transform.position - GameManager.Instance.Player.transform.position).normalized;
                }
                else
                {
                    dir = Vector2.zero;
                }

                if (dir != Vector2.zero)
                {
                    // 인터페이스를 통한 넉백 실행
                    knockbackable.ApplyKnockBack(dir);
                }
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            // GameManager를 통해 풀 시스템에서 경험치 아이템 생성
            if (GameManager.Instance != null && GameManager.Instance.Pool != null)
            {
                ExperienceItem expItem = GameManager.Instance.Pool.GetExperienceItem();
                if (expItem != null)
                {
                    expItem.transform.position = transform.position;
                }
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
        /// 플레이어와 접촉 중일 때 데미지를 입힘
        /// </summary>
        private void OnCollisionStay2D(Collision2D collision)
        {
            // 충돌 대상이 플레이어 태그를 가지고 있다면
            if (collision.gameObject.CompareTag("Player"))
            {
                // IDamageable 인터페이스를 찾아 데미지를 줍니다.
                IDamageable hit = collision.gameObject.GetComponent<IDamageable>();
                if (hit != null && data != null)
                {
                    // 자신의 위치를 함께 넘겨 플레이어가 반대 방향으로 넉백되도록 함
                    hit.TakeDamage(data.DamageAmount, transform.position);
                }
            }
        }
    }
}
