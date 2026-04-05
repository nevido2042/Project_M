using System.Collections;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 몬스터의 전체적인 행동을 관리하는 클래스 (체력은 EnemyHealth에서 담당)
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        [Header("데이터 설정")]
        [SerializeField] private EnemyData data; // 공통 능력치 데이터 에셋
        private Move move;         // 이동 컴포넌트 참조
        private Chase chase;       // 추적 AI 참조
        private EnemyHealth health; // 체력 컴포넌트 참조
        private Animator anim;                // 애니메이터 추가
        private Collider2D enemyCollider;     // 콜라이더 추가
        private UnityEngine.Pool.IObjectPool<Enemy> pool; // 자신을 관리하는 풀 참조

        private bool isDead = false;          // 사망 상태 플래그

        // 체력 및 무적 정보 프로퍼티 (기존 호환성 유지)
        public float CurrentHealth => health != null ? health.CurrentHealth : 0f;
        public float MaxHealth => health != null ? health.MaxHealth : 0f;
        public bool IsDead => isDead; // 사망 상태 외부 노출
        public bool IsInvincible => isDead; // 죽는 중에는 무적 판정

        private void Awake()
        {
            move = GetComponent<Move>();
            chase = GetComponent<Chase>();
            health = GetComponent<EnemyHealth>();
            anim = GetComponent<Animator>();
            enemyCollider = GetComponent<Collider2D>();

            // 초기 신호 연동
            if (health != null)
            {
                health.OnDeath += Die;
            }

            // 속도 초기화
            if (data != null && move != null)
            {
                move.Speed = data.MoveSpeed;
            }
        }

        private void OnEnable()
        {
            // 상태 초기화
            isDead = false;
            if (enemyCollider != null) enemyCollider.enabled = true;
            if (move != null) move.enabled = true;
            if (chase != null) chase.enabled = true;
            if (anim != null) anim.SetBool("Dead", false);
            
            // 데이터 연동은 EnemyHealth의 OnEnable에서 처리함
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


        public void Die()
        {
            if (isDead) return;
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            isDead = true;

            // 콜라이더 및 이동/AI 비활성화
            if (enemyCollider != null) enemyCollider.enabled = false;
            if (move != null)
            {
                move.Velocity = Vector2.zero; // 방향 초기화
                move.enabled = false;
            }
            if (chase != null) chase.enabled = false;
            
            // 즉시 속도 정지 (관성 제거)
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            // 사망 애니메이션 실행 (Bool 파라미터 사용)
            if (anim != null) anim.SetBool("Dead", true);

            // 애니메이션이 충분히 재생될 시간 대기 (약 1초)
            yield return new WaitForSeconds(1.0f);

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
            if (isDead) return; // 사망 중엔 충돌 판정 제외

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

        private void OnDestroy()
        {
            if (health != null)
            {
                health.OnDeath -= Die;
            }
        }
    }
}
