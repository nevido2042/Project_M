using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 데미지를 입을 수 있는 대상이 구현해야 할 인터페이스
    /// </summary>
    public interface IDamageable
    {
        bool IsInvincible { get; }
        float CurrentHealth { get; }
        float MaxHealth { get; }
        void TakeDamage(float damage, Vector2? damageSourcePos = null);
        void Die();
    }
}
