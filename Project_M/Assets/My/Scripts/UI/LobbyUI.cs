using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 메인 메뉴(로비)의 활성 상태를 관리하는 컴포넌트
    /// </summary>
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private GameObject content;

        private void Start()
        {
            if (content == null)
            {
                Debug.LogWarning($"[{nameof(LobbyUI)}] Content(Panel)가 할당되지 않았습니다! {gameObject.name}에서 작동하지 않습니다.");
                return;
            }

            // 초기 상태: 로비는 켜져 있음
            content.SetActive(true);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += Hide;
            }
        }

        private void Hide() { if (content != null) content.SetActive(false); }
    }
}
