using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 원거리 무기의 공격 속도(연사 속도)를 강화하는 업그레이드
    /// </summary>
    [CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "Upgrades/FireRate")]
    public class FireRateUpgradeData : UpgradeData
    {
        [SerializeField] private float speedIncreaseRate = 0.1f; // 각 레벨당 10% 증가 예시

        public override void ApplyUpgrade(Player player)
        {
            RangedWeapon weapon = player.GetComponentInChildren<RangedWeapon>();
            if (weapon != null)
            {
                currentLevel++;
                weapon.FireRate *= (1f - speedIncreaseRate);
                Debug.Log($"[Upgrade] 연사 속도 강화 (레벨 {currentLevel}): 현재 간격 {weapon.FireRate}");
            }
        }
    }
}
