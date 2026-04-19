using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 몬스터의 전체적인 행동을 관리하는 클래스 (체력은 EnemyHealth에서 담당)
    /// </summary>
    public class Enemy : MonoBehaviour, IPoolable
    {
        [Header("데이터 설정")]
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("data")] private EnemyData m_Data; // 공통 능력치 데이터 에셋
        private Move m_Move;         // 이동 컴포넌트 참조
        private Chase m_Chase;       // 추적 AI 참조
        private EnemyHealth m_Health; // 체력 컴포넌트 참조
        private Animator m_Anim;                // 애니메이터 추가
        private Collider2D m_EnemyCollider;     // 콜라이더 추가
        private UnityEngine.Pool.IObjectPool<Enemy> m_Pool; // 풀 참조 직접 관리

        // 체력 및 무적 정보 프로퍼티 (기존 호환성 유지)
        public float CurrentHealth => m_Health != null ? m_Health.CurrentHealth : 0f;
        public float MaxHealth => m_Health != null ? m_Health.MaxHealth : 0f;
        public bool IsDead => m_Health != null && m_Health.IsDead; // EnemyHealth에서 상태 가져옴
        public bool IsInvincible => m_Health != null && m_Health.IsInvincible; 

        private void Awake()
        {
            m_Move = GetComponent<Move>();
            m_Chase = GetComponent<Chase>();
            m_Health = GetComponent<EnemyHealth>();
            m_Anim = GetComponent<Animator>();
            m_EnemyCollider = GetComponent<Collider2D>();

            // 속도 초기화
            if (m_Data != null && m_Move != null)
            {
                m_Move.Speed = m_Data.MoveSpeed;
            }
        }

        private void OnEnable()
        {
            // 기본적인 활성화 상태만 보장 (상세한 체력/사망 상태는 EnemyHealth에서 관리)
            if (m_EnemyCollider != null) m_EnemyCollider.enabled = true;
            if (m_Move != null) m_Move.enabled = true;
            if (m_Chase != null) m_Chase.enabled = true;
            if (m_Anim != null) m_Anim.SetBool("Dead", false);
        }

        /// <summary>
        /// 자신이 속한 풀을 설정합니다 (PoolManager에서 호출)
        /// </summary>
        public void SetPool(UnityEngine.Pool.IObjectPool<Enemy> pool)
        {
            m_Pool = pool;
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
                // HealthBase 컴포넌트를 찾아 데미지 정보를 전달합니다.
                HealthBase hit = collision.gameObject.GetComponent<HealthBase>();
                if (hit != null && m_Data != null)
                {
                    // 자신의 위치를 함께 넘겨 플레이어가 반대 방향으로 넉백되도록 함
                    // 몬스터 접촉 데미지는 약간의 넉백(2.0f)을 줌
                    DamageData dData = new DamageData(m_Data.DamageAmount, transform.position, 2f);
                    hit.TakeDamage(dData);
                }
            }
        }

        /// <summary>
        /// 모든 동작을 중지합니다 (게임 오버 시 호출)
        /// </summary>
        public void StopAction()
        {
            if (m_Move != null)
            {
                m_Move.Velocity = Vector2.zero;
                m_Move.enabled = false;
            }
            if (m_Chase != null) m_Chase.enabled = false;
            
            // 물리 연산 고정 (튕김 방지 및 위치 고정)
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0;
                rb.bodyType = RigidbodyType2D.Static; 
            }

            // 애니메이터 정지
            if (m_Anim != null) m_Anim.speed = 0;

            Debug.Log($"[{gameObject.name}] StopAction 실행됨");
        }

        /// <summary>
        /// 풀로 반납합니다 (IPoolable 인터페이스 구현)
        /// </summary>
        public void Release()
        {
            if (m_Pool != null)
                m_Pool.Release(this);
            else
                gameObject.SetActive(false);
        }
    }
}
