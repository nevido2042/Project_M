using UnityEngine;

/// <summary>
/// 맵 타일이나 오브젝트가 플레이어와 멀어지면 앞쪽으로 재배치(Reposition)하여 무한 맵을 구현하는 컴포넌트
/// </summary>
public class Reposition : MonoBehaviour
{
    [SerializeField] private float tileSize = 10f; // 타일 한 칸의 크기 (사용자 요청: 10)
    private static Transform playerTransform;

    private void Start()
    {
        // 플레이어 트랜스폼 정적 참조 (모든 타일이 공유)
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

        if (playerTransform == null) return;

        Vector3 playerPos = playerTransform.position;
        Vector3 myPos = transform.position;

        // 플레이어와 타일 간의 상대 거리 계산
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        // 플레이어의 위치에 따른 재배치 방향 결정 (1 또는 -1)
        float dirX = playerPos.x > myPos.x ? 1 : -1;
        float dirY = playerPos.y > myPos.y ? 1 : -1;

        if (transform.CompareTag("Ground"))
        {
            // 3x3 구성을 위해 가로/세로 거리 차이에 따라 재배치
            if (diffX > diffY)
            {
                // 타일 3칸 너비(tileSize * 3 = 30)만큼 이동
                transform.Translate(Vector3.right * dirX * tileSize * 3);
            }
            else if (diffX < diffY)
            {
                // 타일 3칸 높이(tileSize * 3 = 30)만큼 이동
                transform.Translate(Vector3.up * dirY * tileSize * 3);
            }
        }
        else if (transform.CompareTag("Enemy"))
        {
            // 몬스터 재배치 로직 (필요 시 추가)
        }
    }
}
