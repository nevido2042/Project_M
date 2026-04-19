using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 게임 오버 이벤트를 구독하여 사망 메뉴 UI를 제어하는 컴포넌트
    /// </summary>
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("content")] private GameObject m_Content;

        private void Start()
        {
            if (m_Content == null)
            {
                Debug.LogWarning($"[{nameof(GameOverUI)}] Content(Panel)가 할당되지 않았습니다! {gameObject.name}에서 작동하지 않습니다.");
                return;
            }

            // 초기 상태: 숨김
            m_Content.SetActive(false);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += Hide;
                GameManager.Instance.OnGameOver += Show;
            }
        }

        private void Show() { if (m_Content != null) m_Content.SetActive(true); }
        private void Hide() { if (m_Content != null) m_Content.SetActive(false); }

        /// <summary>
        /// [다시 하기] 버튼 등에 연결
        /// </summary>
        public void OnClickRestart()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestartScene();
            }
        }
    }
}
