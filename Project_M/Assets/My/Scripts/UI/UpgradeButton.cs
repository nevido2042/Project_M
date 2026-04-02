using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hero
{
    /// <summary>
    /// 강화 선택창의 개별 버튼을 관리하는 컴포넌트
    /// </summary>
    public class UpgradeButton : MonoBehaviour
    {
        [Header("UI 참조")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI levelText;

        private UpgradeData upgradeData;
        private System.Action<UpgradeData> onSelected;

        /// <summary>
        /// 버튼에 강화 데이터를 주입하고 UI를 갱신합니다.
        /// </summary>
        public void SetUpgrade(UpgradeData data, System.Action<UpgradeData> callback)
        {
            upgradeData = data;
            onSelected = callback;

            if (iconImage != null) iconImage.sprite = data.icon;
            if (nameText != null) nameText.text = data.upgradeName;
            if (descriptionText != null) descriptionText.text = data.description;
            if (levelText != null) levelText.text = $"Lv.{data.currentLevel + 1}";
            
            // 만약 최대 레벨이라면 레벨 텍스트를 MAX로 변경하는 디테일 추가
            if (data.currentLevel >= data.maxLevel)
            {
                if (levelText != null) levelText.text = "MAX";
            }
        }

        /// <summary>
        /// 버튼 클릭 시 호출되는 메서드 (UI Button 컴포넌트에 연결 필요)
        /// </summary>
        public void OnClick()
        {
            if (upgradeData != null)
            {
                onSelected?.Invoke(upgradeData);
            }
        }
    }
}
