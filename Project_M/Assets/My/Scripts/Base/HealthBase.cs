using System;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 모든 체력 시스템의 기반이 되는 추상 클래스
    /// </summary>
    public abstract class HealthBase : MonoBehaviour
    {
        [Header("체력 설정 (Base)")]
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("maxHealth")] protected float m_MaxHealth = 100f;
        protected float m_CurrentHealth;

        public event Action OnDeath;
        public event Action<DamageData> OnDamaged;
        public event Action<float, float> OnHealthChanged; // 체력 변경 이벤트 추가 (current, max)

        public float CurrentHealth => m_CurrentHealth;
        public float MaxHealth => m_MaxHealth;
        public abstract bool IsInvincible { get; }

        protected virtual void Awake()
        {
            m_CurrentHealth = m_MaxHealth;
            InvokeHealthChanged();
        }

        /// <summary>
        /// 체력 변경 이벤트를 호출합니다. 자식 클래스에서도 안전하게 호출할 수 있도록 제공합니다.
        /// </summary>
        protected void InvokeHealthChanged()
        {
            OnHealthChanged?.Invoke(m_CurrentHealth, m_MaxHealth);
        }

        /// <summary>
        /// 데미지를 입었을 때의 기본 로직
        /// </summary>
        public virtual void TakeDamage(DamageData data)
        {
            if (IsInvincible || m_CurrentHealth <= 0) return;

            m_CurrentHealth -= data.damage;

            // 공통 사운드 효과
            if (GameManager.Instance?.Audio != null)
                GameManager.Instance.Audio.PlaySFX(SfxType.Hit);

            // 이벤트 호출 (외부에서 넉백 등을 처리)
            OnDamaged?.Invoke(data);
            InvokeHealthChanged();

            if (m_CurrentHealth <= 0)
            {
                Die();
            }
        }


        /// <summary>
        /// 인터페이스 구현 및 캐릭터 사망 로직 호출
        /// </summary>
        public virtual void Die()
        {
            if (GameManager.Instance?.Audio != null)
                GameManager.Instance.Audio.PlaySFX(SfxType.Die);
            
            OnDeath?.Invoke();
        }

        /// <summary>
        /// 체력을 회복합니다.
        /// </summary>
        public virtual void Heal(float amount)
        {
            if (m_CurrentHealth <= 0) return;
            m_CurrentHealth = Mathf.Min(m_CurrentHealth + amount, m_MaxHealth);
            OnHealthChanged?.Invoke(m_CurrentHealth, m_MaxHealth);
        }
    }
}
