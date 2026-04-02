using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 게임 중 일시 정지 메뉴(Pause Menu)의 버튼들을 관리하는 전용 클래스
    /// </summary>
    public class PauseMenuUI : MonoBehaviour
    {
        /// <summary>
        /// [Resume] 버튼 클릭 시 게임 재개
        /// </summary>
        public void OnClickResume()
        {
            if (GameManager.Instance != null)
            {
                // 자기 자신을 끄고 시간을 정상화하도록 요청
                GameManager.Instance.ResumeGame(this.gameObject);
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
