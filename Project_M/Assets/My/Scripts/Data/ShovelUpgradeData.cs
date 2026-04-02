using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 삽 무기의 개수를 증가시키는 강화 항목
    /// </summary>
    [CreateAssetMenu(fileName = "ShovelUpgrade", menuName = "Upgrade/ShovelCount")]
    public class ShovelUpgradeData : UpgradeData
    {
        public override void ApplyUpgrade(Player player)
        {
            // 플레이어 자식 객체에서 MeleeWeapon 컴포넌트를 찾음
            MeleeWeapon meleeWeapon = player.GetComponentInChildren<MeleeWeapon>();

            if (meleeWeapon != null)
            {
                currentLevel++;
                // 현재 레벨에 맞춰 무기 개수를 설정 (기본 1개 + 강화 레벨)
                meleeWeapon.SetWeaponCount(1 + currentLevel);
                Debug.Log($"[Upgrade] 삽 개수 증가! 현재 레벨: {currentLevel}, 무기 수: {1 + currentLevel}");
            }
        }
    }
}
