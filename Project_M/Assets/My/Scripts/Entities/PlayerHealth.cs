using System;
using System.Collections;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어 전용 체력 관리 컴포넌트
    /// </summary>
    public class PlayerHealth : HealthBase
    {
        [Header("플레이어 특화 설정")]
        [SerializeField] private float invincibilityDuration = 0.5f;
        private bool isInvincible = false;

        public event Action<float, float> OnHealthChanged;

        public override bool IsInvincible => isInvincible;

        protected override void Awake()
        {
            base.Awake();
            // 초기 체력 값을 UI 등에 알리기 위해 이벤트 호출
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public override void TakeDamage(float damage, Vector2? damageSourcePos = null)
        {
            if (isInvincible || currentHealth <= 0) return;

            base.TakeDamage(damage, damageSourcePos);

            // 체력 변화 알림
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth > 0)
            {
                StartCoroutine(InvincibilityRoutine());
            }
        }

        private IEnumerator InvincibilityRoutine()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invincibilityDuration);
            isInvincible = false;
        }

        public override void Heal(float amount)
        {
            base.Heal(amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }
}
