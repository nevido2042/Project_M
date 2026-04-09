using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 게임 중 일시 정지 메뉴(Pause Menu)의 버튼들을 관리하는 전용 클래스
    /// </summary>
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject content; // 일시정지 메뉴 UI 루트

        private void Start()
        {
            if (content == null)
            {
                Debug.LogWarning($"[{nameof(PauseMenuUI)}] Content(Panel)가 할당되지 않았습니다! {gameObject.name}에서 작동하지 않습니다.");
                return;
            }
            
            // 초기 상태: 숨김
            content.SetActive(false);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGamePause += Show;
                GameManager.Instance.OnGameResume += Hide;
                GameManager.Instance.OnGameOver += Hide;
            }
        }

        private void Show() { if (content != null) content.SetActive(true); }
        private void Hide() { if (content != null) content.SetActive(false); }

        /// <summary>
        /// [Resume] 버튼 클릭 시 게임 재개
        /// </summary>
        public void OnClickResume()
        {
            Debug.Log("[PauseMenuUI] Resume 버튼 클릭됨");
            if (GameManager.Instance != null)
            {
                // 게임 재개 요청 (이벤트를 통해 이 UI가 닫힘)
                GameManager.Instance.ResumeGame();
            }
        }

        /// <summary>
        /// [Main Menu / Restart] 버튼 클릭 시 현재 씬 다시 로드
        /// </summary>
        public void OnClickRestart()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestartScene();
            }
        }
        
        /// <summary>
        /// 혹시나 있을 게임 종료 버튼 용 (옵션)
        /// </summary>
        public void OnClickExit()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.QuitGame();
            }
        }
    }
}
