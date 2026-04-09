using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 한 번에 발사되는 총알(산탄)의 개수를 늘려주는 업그레이드
    /// </summary>
    [CreateAssetMenu(fileName = "BulletCountUpgrade", menuName = "Upgrades/BulletCount")]
    public class BulletCountUpgradeData : UpgradeData
    {
        public override void ApplyUpgrade(Player player)
        {
            RangedWeapon weapon = player.GetComponentInChildren<RangedWeapon>();
            if (weapon != null)
            {
                currentLevel++;
                weapon.BulletCount = 1 + currentLevel; // 기본 1개 + 강화 수
                Debug.Log($"[Upgrade] 탄환 수 증가 (레벨 {currentLevel}): 총 {weapon.BulletCount}발");
            }
        }
    }
}
