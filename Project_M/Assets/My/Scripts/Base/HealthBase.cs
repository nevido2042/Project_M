using System;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 모든 체력 시스템의 기반이 되는 추상 클래스
    /// </summary>
    public abstract class HealthBase : MonoBehaviour, IDamageable
    {
        [Header("체력 설정 (Base)")]
        [SerializeField] protected float maxHealth = 100f;
        protected float currentHealth;

        protected IKnockbackable knockbackable;
        protected DamageFlash damageFlash;

        public event Action OnDeath;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public abstract bool IsInvincible { get; }

        protected virtual void Awake()
        {
            knockbackable = GetComponent<IKnockbackable>();
            damageFlash = GetComponent<DamageFlash>();
            currentHealth = maxHealth;
        }

        /// <summary>
        /// 데미지를 입었을 때의 기본 로직
        /// </summary>
        public virtual void TakeDamage(float damage, Vector2? damageSourcePos = null)
        {
            if (IsInvincible || currentHealth <= 0) return;

            currentHealth -= damage;

            // 공통 시각 효과: 깜빡임
            if (damageFlash != null) damageFlash.CallFlash();

            // 공통 물리 효과: 넉백
            ApplyKnockBackEffect(damageSourcePos);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// 넉백 효과를 적용합니다. 자식 클래스에서 방향 계산 로직을 다르게 할 수 있습니다.
        /// </summary>
        protected virtual void ApplyKnockBackEffect(Vector2? damageSourcePos)
        {
            if (knockbackable != null && damageSourcePos.HasValue)
            {
                Vector2 dir = ((Vector2)transform.position - damageSourcePos.Value).normalized;
                knockbackable.ApplyKnockBack(dir);
            }
        }

        /// <summary>
        /// 인터페이스 구현 및 캐릭터 사망 로직 호출
        /// </summary>
        public virtual void Die()
        {
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
