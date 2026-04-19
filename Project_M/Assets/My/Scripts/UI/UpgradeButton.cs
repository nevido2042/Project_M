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
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("iconImage")] private Image m_IconImage;
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("nameText")] private TextMeshProUGUI m_NameText;
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("descriptionText")] private TextMeshProUGUI m_DescriptionText;
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("levelText")] private TextMeshProUGUI m_LevelText;

        private UpgradeData m_UpgradeData;
        private System.Action<UpgradeData> m_OnSelected;

        /// <summary>
        /// 버튼에 강화 데이터를 주입하고 UI를 갱신합니다.
        /// </summary>
        public void SetUpgrade(UpgradeData data, System.Action<UpgradeData> callback)
        {
            m_UpgradeData = data;
            m_OnSelected = callback;

            if (m_IconImage != null) m_IconImage.sprite = data.icon;
            if (m_NameText != null) m_NameText.text = data.upgradeName;
            if (m_DescriptionText != null) m_DescriptionText.text = data.description;
            if (m_LevelText != null) m_LevelText.text = $"Lv.{data.currentLevel + 1}";
            
            // 만약 최대 레벨이라면 레벨 텍스트를 MAX로 변경하는 디테일 추가
            if (data.currentLevel >= data.maxLevel)
            {
                if (m_LevelText != null) m_LevelText.text = "MAX";
            }
        }

        /// <summary>
        /// 버튼 클릭 시 호출되는 메서드 (UI Button 컴포넌트에 연결 필요)
        /// </summary>
        public void OnClick()
        {
            if (m_UpgradeData != null)
            {
                m_OnSelected?.Invoke(m_UpgradeData);
            }
        }
    }
}
