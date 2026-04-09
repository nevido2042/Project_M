using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 아이템을 탐지하고 끌어당기는 자석 범위를 확장하는 업그레이드
    /// </summary>
    [CreateAssetMenu(fileName = "PickupRangeUpgrade", menuName = "Upgrades/PickupRange")]
    public class PickupRangeUpgradeData : UpgradeData
    {
        [SerializeField] private float rangeIncrease = 1.0f; // 레벨당 1m 증가 예시

        public override void ApplyUpgrade(Player player)
        {
            PickupCollector collector = player.GetComponentInChildren<PickupCollector>();
            if (collector != null)
            {
                currentLevel++;
                collector.PickupRadius += rangeIncrease;
                Debug.Log($"[Upgrade] 획득 범위 강화 (레벨 {currentLevel}): 현재 {collector.PickupRadius}m");
            }
        }
    }
}
