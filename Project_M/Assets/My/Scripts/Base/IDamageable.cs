/// <summary>
/// 데미지를 입을 수 있는 대상이 구현해야 할 인터페이스
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// 대상의 무적 상태 여부를 확인합니다.
    /// </summary>
    bool IsInvincible { get; }

    /// <summary>
    /// 현재 체력을 가져옵니다.
    /// </summary>
    float CurrentHealth { get; }

    /// <summary>
    /// 최대 체력을 가져옵니다.
    /// </summary>
    float MaxHealth { get; }

    /// <summary>
    /// 대상에게 데미지를 입힙니다.
    /// </summary>
    /// <param name="damage">입힐 데미지 양</param>
    void TakeDamage(float damage, UnityEngine.Vector2? damageSourcePos = null);

    /// <summary>
    /// 대상이 사망했을 때 호출되는 메서드입니다.
    /// </summary>
    void Die();
}
