using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

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
        
        // 시작 시 조이스틱 숨김
        joystickCanvasGroup.alpha = 0f;
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
