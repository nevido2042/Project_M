using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 게임 상태 이벤트에 따라 인게임 HUD(HP바, 레벨 등)를 제어하는 컴포넌트
    /// </summary>
    public class HUDUI : MonoBehaviour
    {
        [SerializeField] private GameObject content; // 실제 HUD 내용물 오브젝트

        private void Start()
        {
            if (content == null) content = gameObject;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += Show;
                GameManager.Instance.OnGameOver += Hide;
            }
        }

        private void Show() => content.SetActive(true);
        private void Hide() => content.SetActive(false);
    }
}
