using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    private Move movement;

    [SerializeField]
    private InputActionAsset inputAsset;

    private void Awake()
    {
        movement = GetComponent<Move>();
    }

    private void OnEnable()
    {
        // 에셋 내의 모든 액션 맵을 활성화합니다.
        inputAsset?.Enable();
    }

    private void OnDisable()
    {
        // 에셋 내의 모든 액션 맵을 비활성화합니다.
        inputAsset?.Disable();
    }

    private void Update()
    {
        if (inputAsset != null)
        {
            // "Move" 액션을 찾아 벡터 값을 읽어온 뒤 Move 컴포넌트에 전달합니다.
            var moveAction = inputAsset.FindAction("Move");
            if (moveAction != null)
            {
                movement.Velocity = moveAction.ReadValue<Vector2>();
            }
        }
    }
}
