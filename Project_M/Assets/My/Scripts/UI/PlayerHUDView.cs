using UnityEngine;
using Hero;

namespace Hero
{
    /// <summary>
    /// 화면에 고정된 메인 HUD 컨테이너의 가시성(Show/Hide)을 관리하는 Controller.
    /// 세부 UI 요소(HP, EXP 등)는 각각의 전용 View 스크립트에서 처리합니다.
    /// </summary>
    public class PlayerHUDView : MonoBehaviour
    {
        [Header("UI 패널 연결")]
        [SerializeField] private GameObject content; // 실제 HUD 내용물 오브젝트 (부모 패널)

        private void Start()
        {
            if (content == null)
            {
                Debug.LogWarning($"[{nameof(PlayerHUDView)}] Content(Panel)가 할당되지 않았습니다! {gameObject.name}에서 작동하지 않습니다.");
                return;
            }
            
            // 초기 상태: 게임 시작 전에는 숨김
            content.SetActive(false);

            // 게임 상태 이벤트 구독
            if (GameManager.HasInstance)
            {
                GameManager.Instance.OnGameStart += Show;
                GameManager.Instance.OnGameOver += Hide;
            }
        }

        public void Show() { if (content != null) content.SetActive(true); }
        public void Hide() { if (content != null) content.SetActive(false); }

        private void OnDestroy()
        {
            if (GameManager.HasInstance)
            {
                GameManager.Instance.OnGameStart -= Show;
                GameManager.Instance.OnGameOver -= Hide;
            }
        }
    }
}

