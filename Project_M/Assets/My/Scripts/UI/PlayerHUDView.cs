using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro가 없을 경우 일반 Text로 변경 가능
using Hero;

/// <summary>
/// 화면에 고정된 메인 HUD (HP, EXP, Level)를 렌더링하는 View
/// </summary>
public class PlayerHUDView : MonoBehaviour
{
    [Header("UI 요소 연결")]
    [SerializeField] private Slider healthSlider; // 체력 슬라이더
    [SerializeField] private Slider expSlider;    // 경험치 슬라이더
    [SerializeField] private TextMeshProUGUI levelText; // 레벨 텍스트

    private PlayerStatusViewModel viewModel;

    private void Start()
    {
        // 플레이어를 자동으로 찾아 뷰모델 초기화
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Player player = playerObj.GetComponent<Player>();
            if (player != null)
            {
                viewModel = new PlayerStatusViewModel(player);
                SubscribeToViewModelEvents();
                InitializeUI(); // 초기 상태 반영
            }
        }
    }

    private void OnDestroy()
    {
        UnsubscribeFromViewModelEvents();
    }

    private void SubscribeToViewModelEvents()
    {
        if (viewModel == null) return;
        viewModel.OnHealthRatioChanged += UpdateHealthSlider;
        viewModel.OnExpRatioChanged += UpdateExpSlider;
        viewModel.OnLevelTextChanged += UpdateLevelText;
    }

    private void UnsubscribeFromViewModelEvents()
    {
        if (viewModel == null) return;
        viewModel.OnHealthRatioChanged -= UpdateHealthSlider;
        viewModel.OnExpRatioChanged -= UpdateExpSlider;
        viewModel.OnLevelTextChanged -= UpdateLevelText;
    }

    private void InitializeUI()
    {
        if (viewModel == null) return;
        UpdateHealthSlider(viewModel.HealthRatio);
        UpdateExpSlider(viewModel.ExpRatio);
        UpdateLevelText(viewModel.LevelText);
    }

    private void UpdateHealthSlider(float ratio)
    {
        if (healthSlider != null) healthSlider.value = ratio;
    }

    private void UpdateExpSlider(float ratio)
    {
        if (expSlider != null) expSlider.value = ratio;
    }

    private void UpdateLevelText(string text)
    {
        if (levelText != null) levelText.text = text;
    }

}
