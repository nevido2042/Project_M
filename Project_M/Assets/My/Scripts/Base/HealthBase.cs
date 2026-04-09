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
        [SerializeField] protected float maxHealth = 100f;
        protected float currentHealth;

        public event Action OnDeath;
        public event Action<DamageData> OnDamaged; // 피격 이벤트 추가

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public abstract bool IsInvincible { get; }

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        /// <summary>
        /// 데미지를 입었을 때의 기본 로직
        /// </summary>
        public virtual void TakeDamage(DamageData data)
        {
            if (IsInvincible || currentHealth <= 0) return;

            currentHealth -= data.damage;

            // 공통 사운드 효과
            if (GameManager.Instance?.Audio != null)
                GameManager.Instance.Audio.PlaySFX(SfxType.Hit);

            // 이벤트 호출 (외부에서 넉백 등을 처리)
            OnDamaged?.Invoke(data);

            if (currentHealth <= 0)
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
            if (currentHealth <= 0) return;
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        }
    }
}
