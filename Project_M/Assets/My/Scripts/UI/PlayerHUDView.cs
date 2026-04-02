using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro가 없을 경우 일반 Text로 변경 가능

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
            }
        }
    }

    private void Update()
    {
        if (viewModel == null) return;

        // 1. 체력 업데이트
        if (healthSlider != null)
        {
            healthSlider.value = viewModel.HealthRatio;
        }

        // 2. 경험치 업데이트
        if (expSlider != null)
        {
            expSlider.value = viewModel.ExpRatio;
        }

        // 3. 레벨 텍스트 업데이트
        if (levelText != null)
        {
            levelText.text = viewModel.LevelText;
        }
        
        // 4. 사망 상태 시 시각적 반응 (필요 시 HUD를 빨갛게 하거나 숨기는 등)
        if (viewModel.IsDead)
        {
            // 예: 캔버스 알파값을 낮추는 등의 로직 추가 가능
        }
    }
}
