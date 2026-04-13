using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어의 이동 속도를 증가시키는 강화 항목
    /// </summary>
    [CreateAssetMenu(fileName = "MoveSpeedUpgrade", menuName = "Upgrades/MoveSpeed")]
    public class MoveSpeedUpgradeData : UpgradeData
    {
        [Header("속도 설정")]
        [SerializeField] private float speedMultiplier = 1.1f; // 각 레벨당 10% 증가

        public override void ApplyUpgrade(Player player)
        {
            // 플레이어 객체에서 Move 컴포넌트를 직접 참조
            Move move = player.GetComponent<Move>();

            if (move != null)
            {
                currentLevel++;
                // 기존 속도에 배율을 곱하여 속도 상승
                move.Speed *= speedMultiplier;
                Debug.Log($"[Upgrade] 이동 속도 증가! 현재 레벨: {currentLevel}, 현재 속도: {move.Speed:F2}");
            }
        }
    }
}
