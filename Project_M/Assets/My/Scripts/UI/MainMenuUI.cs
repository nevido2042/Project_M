using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 메인 메뉴의 버튼 클릭 이벤트를 관리하는 로직 클래스
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        /// <summary>
        /// [게임 시작] 버튼에 연결할 메서드
        /// </summary>
        public void OnClickStart()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
        }

        /// <summary>
        /// [게임 종료] 버튼에 연결할 메서드
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
