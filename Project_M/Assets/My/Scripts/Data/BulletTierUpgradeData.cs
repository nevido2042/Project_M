using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 탄환의 티어(공격력 + 외형)를 업그레이드하는 클래스
    /// </summary>
    [CreateAssetMenu(fileName = "BulletTierUpgrade", menuName = "Upgrades/BulletTier")]
    public class BulletTierUpgradeData : UpgradeData
    {
        public override void ApplyUpgrade(Player player)
        {
            RangedWeapon weapon = player.GetComponentInChildren<RangedWeapon>();
            if (weapon != null)
            {
                currentLevel++;
                weapon.CurrentTier = currentLevel; // 티어를 현재 레벨에 동기화
                Debug.Log($"[Upgrade] 탄환 티어 상승: 현재 {currentLevel}단계");
            }
        }
    }
}
