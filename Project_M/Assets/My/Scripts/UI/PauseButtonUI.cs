using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 게임 중 일시정지 버튼을 관리하는 컴포넌트
    /// </summary>
    public class PauseButtonUI : MonoBehaviour
    {
        [SerializeField] private GameObject content;

        private void Start()
        {
            if (content == null)
            {
                Debug.LogWarning($"[{nameof(PauseButtonUI)}] Content(Button)가 할당되지 않았습니다! {gameObject.name}에서 작동하지 않습니다.");
                return;
            }

            // 초기 상태: 게임 시작 전에는 숨김
            content.SetActive(false);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += Show;
                GameManager.Instance.OnGamePause += Hide;
                GameManager.Instance.OnGameResume += Show;
                GameManager.Instance.OnGameOver += Hide;
            }
        }

        private void Show() { if (content != null) content.SetActive(true); }
        private void Hide() { if (content != null) content.SetActive(false); }

        public void OnClickPause()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PauseGame();
            }
        }
    }
}
