using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어의 레벨업 상태를 감시하고 강화 시스템을 트리거하는 매니저 클래스
    /// </summary>
    public class UpgradeManager : MonoBehaviour
    {
        [Header("연동 대상")]
        [SerializeField] private Player player;        // 대상 플레이어
        [SerializeField] private UpgradeUI upgradeUI;  // 호출할 강화 UI

        private void Start()
        {
            // 이제 플레이어 정보는 GameManager를 통해 중앙 집중형으로 관리합니다.
            if (GameManager.Instance != null)
            {
                player = GameManager.Instance.Player;
            }

            // 이벤트 구독
            if (player != null)
            {
                player.OnLevelChanged += HandleLevelUp;
                Debug.Log("[Upgrade] 강화 매니저가 플레이어의 레벨업 감시를 시작했습니다.");
            }
        }

        private void OnDestroy()
        {
            // 오브젝트 파괴 시 이벤트 구독 해제 (메모리 누수 방지)
            if (player != null)
            {
                player.OnLevelChanged -= HandleLevelUp;
            }
        }

        /// <summary>
        /// 레벨업 이벤트 발생 시 호출되는 핸들러
        /// </summary>
        /// <param name="level">새로운 레벨</param>
        private void HandleLevelUp(int level)
        {
            // 게임 시작 직후(Level 1)에는 강화창을 띄우지 않도록 예외 처리
            if (level <= 1) return;

            Debug.Log($"[Upgrade] 레벨업 감지! ({level}) 강화 선택창을 엽니다.");

            if (upgradeUI != null)
            {
                upgradeUI.Show(player);
            }
            else
            {
                Debug.LogError("[Upgrade] 연결된 UpgradeUI가 없습니다!");
            }
        }
    }
}
