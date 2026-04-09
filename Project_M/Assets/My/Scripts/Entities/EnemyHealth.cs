using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Hero
{
    /// <summary>
    /// 몬스터 전용 체력 관리 컴포넌트
    /// </summary>
    public class EnemyHealth : HealthBase, IPoolable
    {
        [Header("몬스터 데이터")]
        [SerializeField] private EnemyData data;

        private IObjectPool<Enemy> pool;
        private Enemy enemy;
        private Animator anim;
        private Collider2D enemyCollider;
        private Move move;
        private Chase chase;
        private bool isDead = false;

        public bool IsDead => isDead;
        public override bool IsInvincible => isDead;

        protected override void Awake()
        {
            if (data != null)
            {
                maxHealth = data.MaxHealth;
            }
            base.Awake();

            enemy = GetComponent<Enemy>();
            anim = GetComponent<Animator>();
            enemyCollider = GetComponent<Collider2D>();
            move = GetComponent<Move>();
            chase = GetComponent<Chase>();
        }

        public void SetPool(IObjectPool<Enemy> pool)
        {
            this.pool = pool;
        }

        private void OnEnable()
        {
            // 풀에서 재활용될 때 체력 회복
            if (data != null)
            {
                maxHealth = data.MaxHealth;
                currentHealth = maxHealth;
            }
            isDead = false;
        }

        public override void Die()
        {
            if (isDead) return;
            base.Die(); // 사운드 및 OnDeath 호출
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            isDead = true;

            // 컴포넌트 비활성화
            if (enemyCollider != null) enemyCollider.enabled = false;
            if (move != null)
            {
                move.Velocity = Vector2.zero;
                move.enabled = false;
            }
            if (chase != null) chase.enabled = false;

            // 즉시 속도 정지
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            // 사망 애니메이션
            if (anim != null) anim.SetBool("Dead", true);

            // 킬 수 증가
            if (GameManager.Instance != null && GameManager.Instance.Spawner != null)
            {
                GameManager.Instance.Spawner.AddKill();
            }

            yield return new WaitForSeconds(1.0f);

            // 레벨업 시 경험치 생성
            if (GameManager.Instance != null && GameManager.Instance.Pool != null)
            {
                ExperienceItem expItem = GameManager.Instance.Pool.GetExperienceItem();
                if (expItem != null)
                {
                    expItem.transform.position = transform.position;
                }
            }

            Release();
        }

        public override void TakeDamage(float damage, Vector2? damageSourcePos = null)
        {
            base.TakeDamage(damage, damageSourcePos);

            // 피격 애니메이션 트리거 발동
            if (!isDead && anim != null)
            {
                anim.SetTrigger("Hit");
            }
        }

        public void Release()
        {
            if (pool != null)
            {
                pool.Release(enemy);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        protected override void ApplyKnockBackEffect(Vector2? damageSourcePos)
        {
            // 몬스터 특유의 넉백 방향 보정 로직
            Vector2 dir;
            if (damageSourcePos.HasValue)
            {
                dir = ((Vector2)transform.position - damageSourcePos.Value).normalized;
            }
            else if (GameManager.Instance != null && GameManager.Instance.Player != null)
            {
                // 소스 위치가 없을 경우 플레이어 위치를 기준으로 반대 방향 계산
                dir = (transform.position - GameManager.Instance.Player.transform.position).normalized;
            }
            else
            {
                dir = Vector2.zero;
            }

            if (dir != Vector2.zero && knockbackable != null)
            {
                knockbackable.ApplyKnockBack(dir);
            }
        }
    }
}
