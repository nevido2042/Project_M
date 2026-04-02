using UnityEngine;

/// <summary>
/// 트리거 영역을 벗어나는 오브젝트를 감지하여 알림을 보내는 컴포넌트
/// </summary>
public class Reposition : MonoBehaviour
{
    private static Transform playerTransform;
    private IRepositionable repositionHandler;

    private void Awake()
    {
        // 동일한 오브젝트에 부착된 구체적인 재배치 행동을 찾습니다.
        repositionHandler = GetComponent<IRepositionable>();
    }

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // "Area" 태그를 가진 트리거를 벗어날 때 실행
        if (!collision.CompareTag("Area")) return;

        if (playerTransform == null || repositionHandler == null) return;

        // 구체적인 재배치 로직을 호출합니다.
        repositionHandler.Reposition(playerTransform.position);
    }
}
