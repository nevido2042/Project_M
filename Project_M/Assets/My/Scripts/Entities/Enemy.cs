using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 몬스터의 전체적인 행동을 관리하는 클래스 (체력은 EnemyHealth에서 담당)
    /// </summary>
    public class Enemy : MonoBehaviour, IPoolable
    {
        [Header("데이터 설정")]
        [SerializeField] private EnemyData data; // 공통 능력치 데이터 에셋
        private Move move;         // 이동 컴포넌트 참조
        private Chase chase;       // 추적 AI 참조
        private EnemyHealth health; // 체력 컴포넌트 참조
        private Animator anim;                // 애니메이터 추가
        private Collider2D enemyCollider;     // 콜라이더 추가

        // 체력 및 무적 정보 프로퍼티 (기존 호환성 유지)
        public float CurrentHealth => health != null ? health.CurrentHealth : 0f;
        public float MaxHealth => health != null ? health.MaxHealth : 0f;
        public bool IsDead => health != null && health.IsDead; // EnemyHealth에서 상태 가져옴
        public bool IsInvincible => health != null && health.IsInvincible; 

        private void Awake()
        {
            move = GetComponent<Move>();
            chase = GetComponent<Chase>();
            health = GetComponent<EnemyHealth>();
            anim = GetComponent<Animator>();
            enemyCollider = GetComponent<Collider2D>();

            // 속도 초기화
            if (data != null && move != null)
            {
                move.Speed = data.MoveSpeed;
            }
        }

        private void OnEnable()
        {
            // 기본적인 활성화 상태만 보장 (상세한 체력/사망 상태는 EnemyHealth에서 관리)
            if (enemyCollider != null) enemyCollider.enabled = true;
            if (move != null) move.enabled = true;
            if (chase != null) chase.enabled = true;
            if (anim != null) anim.SetBool("Dead", false);
        }

        /// <summary>
        /// 자신이 속한 풀을 설정합니다 (PoolManager에서 호출)
        /// </summary>
        public void SetPool(UnityEngine.Pool.IObjectPool<Enemy> pool)
        {
            if (health != null) health.SetPool(pool);
        }

        private void Start()
        {
            // 이제 플레이어 정보 캐싱은 Reposition 컴포넌트에서 통합 관리합니다.
        }


        /// <summary>
        /// 플레이어와 접촉 중일 때 데미지를 입힘
        /// </summary>
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (IsDead) return; // 사망 중엔 충돌 판정 제외

            // 충돌 대상이 플레이어 태그를 가지고 있다면
            if (collision.gameObject.CompareTag("Player"))
            {
                // IDamageable 인터페이스를 찾아 데미지를 줍니다.
                IDamageable hit = collision.gameObject.GetComponent<IDamageable>();
                if (hit != null && data != null)
                {
                    // 자신의 위치를 함께 넘겨 플레이어가 반대 방향으로 넉백되도록 함
                    hit.TakeDamage(data.DamageAmount, (Vector2)transform.position);
                }
            }
        }

        /// <summary>
        /// 풀로 반납합니다 (IPoolable 인터페이스 구현)
        /// </summary>
        public void Release()
        {
            if (health != null) health.Release();
        }
    }
}
