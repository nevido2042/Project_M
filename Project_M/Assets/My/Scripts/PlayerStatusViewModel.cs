using UnityEngine;

/// <summary>
/// 플레이어의 체력, 경험치, 레벨 등 모든 HUD 정보를 관리하는 통합 뷰모델
/// </summary>
public class PlayerStatusViewModel
{
    private Player player;

    public PlayerStatusViewModel(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// 현재 체력 비율 (0 ~ 1)
    /// </summary>
    public float HealthRatio
    {
        get
        {
            if (player == null || player.MaxHealth <= 0) return 0;
            return Mathf.Clamp01(player.CurrentHealth / player.MaxHealth);
        }
    }

    /// <summary>
    /// 현재 경험치 비율 (0 ~ 1)
    /// </summary>
    public float ExpRatio
    {
        get
        {
            if (player == null || player.NextExp <= 0) return 0;
            return Mathf.Clamp01(player.CurrentExp / player.NextExp);
        }
    }

    /// <summary>
    /// 화면에 표시할 레벨 텍스트 (예: Lv. 10)
    /// </summary>
    public string LevelText
    {
        get
        {
            if (player == null) return "Lv. -";
            return $"Lv. {player.Level}";
        }
    }

    /// <summary>
    /// 플레이어가 사망했는지 여부
    /// </summary>
    public bool IsDead => player == null || player.CurrentHealth <= 0;
}
