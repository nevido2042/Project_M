using System;
using UnityEngine;
using Hero;

/// <summary>
/// 플레이어의 체력, 경험치, 레벨 등 모든 HUD 정보를 관리하는 통합 뷰모델
/// </summary>
public class PlayerStatusViewModel
{
    private Player player;
    
    // View를 위한 가공된 데이터 이벤트
    public event Action<float> OnHealthRatioChanged;
    public event Action<float> OnExpRatioChanged;
    public event Action<string> OnLevelTextChanged;

    public PlayerStatusViewModel(Player player)
    {
        this.player = player;
        
        if (this.player != null)
        {
            // Player의 원본 데이터 변경 이벤트 구독
            this.player.OnHealthChanged += HandleHealthChanged;
            this.player.OnExpChanged += HandleExpChanged;
            this.player.OnLevelChanged += HandleLevelChanged;
        }
    }

    private void HandleHealthChanged(float current, float max)
    {
        float ratio = max > 0 ? Mathf.Clamp01(current / max) : 0;
        OnHealthRatioChanged?.Invoke(ratio);
    }

    private void HandleExpChanged(float current, float next)
    {
        float ratio = next > 0 ? Mathf.Clamp01(current / next) : 0;
        OnExpRatioChanged?.Invoke(ratio);
    }

    private void HandleLevelChanged(int level)
    {
        OnLevelTextChanged?.Invoke($"Lv. {level}");
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
