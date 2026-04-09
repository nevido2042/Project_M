using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Hero
{
    /// <summary>
    /// 모바일 UI 가상 조이스틱 핸들러 (New Input System 연동형)
    /// </summary>
    public class VirtualJoystick : OnScreenControl, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string path;

        protected override string controlPathInternal
        {
            get => path;
            set => path = value;
        }

        [Header("UI 참조")]
        [SerializeField] private RectTransform touchZone;      // 터치 인식 영역
        [SerializeField] private RectTransform joystickVisual; // 조이스틱 배경 시각 요소
        [SerializeField] private RectTransform handleVisual;   // 조이스틱 핸들 시각 요소

        private CanvasGroup joystickCanvasGroup;

        private void Start()
        {
            if (touchZone == null) touchZone = GetComponent<RectTransform>();
            
            joystickCanvasGroup = joystickVisual.GetComponent<CanvasGroup>();
            if (joystickCanvasGroup == null) joystickCanvasGroup = joystickVisual.gameObject.AddComponent<CanvasGroup>();
            
            // 터치 영역(본인)의 상호작용 제어를 위한 CanvasGroup 추가/확인
            CanvasGroup mainGroup = GetComponent<CanvasGroup>();
            if (mainGroup == null) gameObject.AddComponent<CanvasGroup>();
            
            // 초기 상태: 게임 시작 전에는 비활성화
            SetJoystickActive(false);

            // GameManager 이벤트 구독
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += () => SetJoystickActive(true);
                GameManager.Instance.OnGameOver += () => SetJoystickActive(false);
                GameManager.Instance.OnGamePause += () => SetJoystickInteractable(false);
                GameManager.Instance.OnGameResume += () => SetJoystickInteractable(true);
            }
        }

        /// <summary>
        /// 조이스틱의 상호작용 여부를 설정합니다. (일시정지 대응)
        /// </summary>
        private void SetJoystickInteractable(bool isInteractable)
        {
            // 시각적 요소의 상호작용 제어
            if (joystickCanvasGroup != null)
            {
                joystickCanvasGroup.interactable = isInteractable;
                joystickCanvasGroup.blocksRaycasts = isInteractable;
            }

            // 전체 터치 영역(본인)의 상호작용 제어 (일시정지 시 클릭 방지 방해 해결)
            CanvasGroup mainGroup = GetComponent<CanvasGroup>();
            if (mainGroup != null)
            {
                mainGroup.interactable = isInteractable;
                mainGroup.blocksRaycasts = isInteractable;
            }

            if (!isInteractable)
            {
                // 일시정지 시 입력 초기화
                SendValueToControl(Vector2.zero);
                handleVisual.anchoredPosition = Vector2.zero;
            }
        }

        /// <summary>
        /// 조이스틱의 활성 상태를 설정합니다. (GameObject 활성화/비활성화)
        /// </summary>
        private void SetJoystickActive(bool isActive)
        {
            // 조이스틱 전체 오브젝트를 끄거나 켬
            // 여기서는 시각 요소(joystickVisual)가 아닌 전체 기능을 담당하는 gameObject를 제어합니다.
            // (단, 이 스크립트가 붙은 오브젝트가 꺼지면 이벤트를 더 이상 못 받으므로 주의가 필요합니다.
            //  보통은 visual만 끄거나 interactable을 조절하는 것이 안정적입니다.)
            
            if (joystickVisual != null)
            {
                joystickVisual.gameObject.SetActive(isActive);
            }
            
            if (!isActive)
            {
                // 꺼질 때 입력 초기화
                SendValueToControl(Vector2.zero);
                handleVisual.anchoredPosition = Vector2.zero;
                joystickCanvasGroup.alpha = 0f;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // 터치 시 조이스틱 표시
            joystickCanvasGroup.alpha = 1f;

            // 터치한 위치로 조이스틱 배경 이동
            Vector2 localPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(touchZone, eventData.position, eventData.pressEventCamera, out localPos))
            {
                joystickVisual.localPosition = localPos;
            }

            handleVisual.anchoredPosition = Vector2.zero;
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position;
            // 조이스틱 배경 기준으로 핸들 위치 계산
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickVisual, eventData.position, eventData.pressEventCamera, out position))
            {
                float x = position.x / (joystickVisual.sizeDelta.x / 2f);
                float y = position.y / (joystickVisual.sizeDelta.y / 2f);

                Vector2 inputVector = new Vector2(x, y);
                // 입력 벡터 크기를 1로 제한
                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

                // 핸들 시각 요소 위치 업데이트
                handleVisual.anchoredPosition = new Vector2(inputVector.x * (joystickVisual.sizeDelta.x / 3f), inputVector.y * (joystickVisual.sizeDelta.y / 3f));

                // New Input System의 지정된 경로로 데이터 전송
                SendValueToControl(inputVector);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // 터치 해제 시 조이스틱 숨김 및 값 초기화
            joystickCanvasGroup.alpha = 0f;
            handleVisual.anchoredPosition = Vector2.zero;
            
            // 입력 데이터 초기화 전송
            SendValueToControl(Vector2.zero);
        }
    }
}
