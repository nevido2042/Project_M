using UnityEngine;
using UnityEngine.UI;
using Hero;

namespace Hero
{
    /// <summary>
    /// 플레이어의 경험치 변화를 구독하여 슬라이더를 갱신하는 스마트 UI 컴포넌트
    /// </summary>
    public class ExpBarView : MonoBehaviour
    {
        [SerializeField] private Slider expSlider;
        private PlayerStatusViewModel viewModel;

        private void Start()
        {
            if (expSlider == null) expSlider = GetComponent<Slider>();
            if (expSlider == null) expSlider = GetComponentInChildren<Slider>();

            // 플레이어 자동 찾기
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Player player = playerObj.GetComponent<Player>();
                if (player != null)
                {
                    viewModel = new PlayerStatusViewModel(player);
                    viewModel.OnExpRatioChanged += UpdateUI;
                    UpdateUI(viewModel.ExpRatio);
                }
            }
        }

        private void OnDestroy()
        {
            if (viewModel != null)
            {
                viewModel.OnExpRatioChanged -= UpdateUI;
            }
        }

        private void UpdateUI(float ratio)
        {
            if (expSlider != null) expSlider.value = ratio;
        }
    }
}
