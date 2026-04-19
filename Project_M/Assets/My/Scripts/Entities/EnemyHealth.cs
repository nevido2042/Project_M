using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Hero
{
    /// <summary>
    /// 몬스터 전용 체력 관리 컴포넌트
    /// </summary>
    public class EnemyHealth : HealthBase
    {
        [Header("몬스터 데이터")]
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("data")] private EnemyData m_Data;

        private Enemy m_Enemy;
        private Animator m_Anim;
        private Collider2D m_EnemyCollider;
        private Move m_Move;
        private Chase m_Chase;
        private bool m_IsDead = false;

        public bool IsDead => m_IsDead;
        public override bool IsInvincible => m_IsDead;

        protected override void Awake()
        {
            if (m_Data != null)
            {
                m_MaxHealth = m_Data.MaxHealth;
            }
            base.Awake();

            m_Enemy = GetComponent<Enemy>();
            m_Anim = GetComponent<Animator>();
            m_EnemyCollider = GetComponent<Collider2D>();
            m_Move = GetComponent<Move>();
            m_Chase = GetComponent<Chase>();
        }

        private void OnEnable()
        {
            // 풀에서 재활용될 때 체력 회복
            if (m_Data != null)
            {
                m_MaxHealth = m_Data.MaxHealth;
                m_CurrentHealth = m_MaxHealth;
            }
            m_IsDead = false;
        }

        public override void Die()
        {
            if (m_IsDead) return;
            base.Die(); // 사운드 및 OnDeath 호출
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            m_IsDead = true;

            // 컴포넌트 비활성화
            if (m_EnemyCollider != null) m_EnemyCollider.enabled = false;
            if (m_Move != null)
            {
                m_Move.Velocity = Vector2.zero;
                m_Move.enabled = false;
            }
            if (m_Chase != null) m_Chase.enabled = false;

            // 즉시 속도 정지
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            // 사망 애니메이션
            if (m_Anim != null) m_Anim.SetBool("Dead", true);

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

        public override void TakeDamage(DamageData data)
        {
            base.TakeDamage(data);

            // 피격 애니메이션 트리거 발동
            if (!m_IsDead && m_Anim != null)
            {
                m_Anim.SetTrigger("Hit");
            }
        }

        public void Release()
        {
            if (m_Enemy != null)
            {
                m_Enemy.Release();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

    }
}
