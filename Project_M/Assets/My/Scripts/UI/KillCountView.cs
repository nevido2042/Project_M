using UnityEngine;
using TMPro;
using Hero;

namespace Hero
{
    /// <summary>
    /// 킬 카운트 변화를 구독하여 텍스트를 갱신하는 스마트 UI 컴포넌트
    /// </summary>
    public class KillCountView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI killText;
        private KillCountViewModel viewModel;

        private void Start()
        {
            if (killText == null) killText = GetComponent<TextMeshProUGUI>();
            if (killText == null) killText = GetComponentInChildren<TextMeshProUGUI>();

            // EnemySpawner는 GameManager를 통해 접근
            if (GameManager.Instance != null && GameManager.Instance.Spawner != null)
            {
                viewModel = new KillCountViewModel(GameManager.Instance.Spawner);
                viewModel.OnKillCountTextChanged += UpdateUI;
                UpdateUI(viewModel.KillCountText);
            }
        }

        private void OnDestroy()
        {
            if (viewModel != null)
            {
                viewModel.OnKillCountTextChanged -= UpdateUI;
                viewModel.Dispose();
            }
        }

        private void UpdateUI(string text)
        {
            if (killText != null) killText.text = text;
        }
    }
}
