using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 모든 강화 항목의 기본이 되는 추상 ScriptableObject 클래스
    /// </summary>
    public abstract class UpgradeData : ScriptableObject
    {
        [Header("기본 정보")]
        public string upgradeName;       // 강화 이름
        [TextArea] public string description; // 강화 설명
        public Sprite icon;              // UI 표시 아이콘
        
        [Header("레벨 설정")]
        public int maxLevel = 5;         // 최대 강화 레벨
        public int currentLevel = 0;     // 현재 강화 레벨

        /// <summary>
        /// 강화를 실제로 적용하는 메서드 (자식 클래스에서 구현)
        /// </summary>
        /// <param name="player">강화를 적용할 플레이어 객체</param>
        public abstract void ApplyUpgrade(Player player);

        /// <summary>
        /// 강화 레벨을 초기화합니다. (게임 시작 시 필수 호출)
        /// </summary>
        public virtual void ResetLevel()
        {
            currentLevel = 0;
        }

        /// <summary>
        /// 다음 레벨 강화가 가능한지 확인합니다.
        /// </summary>
        public bool CanUpgrade => currentLevel < maxLevel;
    }
}
