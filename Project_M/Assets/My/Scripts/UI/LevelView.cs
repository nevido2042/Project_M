using UnityEngine;
using TMPro;
using Hero;

namespace Hero
{
    /// <summary>
    /// 플레이어의 레벨 변화를 구독하여 텍스트를 갱신하는 스마트 UI 컴포넌트
    /// </summary>
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        private PlayerStatusViewModel viewModel;

        private void Start()
        {
            if (levelText == null) levelText = GetComponent<TextMeshProUGUI>();
            if (levelText == null) levelText = GetComponentInChildren<TextMeshProUGUI>();

            // 플레이어 자동 찾기
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Player player = playerObj.GetComponent<Player>();
                if (player != null)
                {
                    viewModel = new PlayerStatusViewModel(player);
                    viewModel.OnLevelTextChanged += UpdateUI;
                    UpdateUI(viewModel.LevelText);
                }
            }
        }

        private void OnDestroy()
        {
            if (viewModel != null)
            {
                viewModel.OnLevelTextChanged -= UpdateUI;
            }
        }

        private void UpdateUI(string text)
        {
            if (levelText != null) levelText.text = text;
        }
    }
}
