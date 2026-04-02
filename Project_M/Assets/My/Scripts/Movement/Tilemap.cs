using UnityEngine;

/// <summary>
/// 맵 타일 재배치를 담당하는 클래스
/// </summary>
public class MapReposition : MonoBehaviour, IRepositionable
{
    [SerializeField] private float tileSize = 10f; // 타일 한 칸의 크기
    private static Transform playerTransform;

    private void Start()
    {
        // 플레이어 정보 캐싱
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }
    }

    /// <summary>
    /// 플레이어의 영역(Area)을 벗어날 때 재배치를 수행합니다.
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // "Area" 태그를 가진 트리거를 벗어날 때 실행
        if (!collision.CompareTag("Area")) return;

        if (playerTransform != null)
        {
            // 인터페이스를 상속받은 자기 자신의 재배치 로직을 호출합니다.
            Reposition(playerTransform.position);
        }
    }

    /// <summary>
    /// 플레이어의 위치에 따라 타일을 반대편으로 재배치합니다.
    /// </summary>
    public void Reposition(Vector3 playerPos)
    {
        Vector3 myPos = transform.position;

        // 플레이어와 타일 간의 상대 거리 계산
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        // 플레이어의 위치에 따른 재배치 방향 결정
        float dirX = playerPos.x > myPos.x ? 1 : -1;
        float dirY = playerPos.y > myPos.y ? 1 : -1;

        // X축 거리차이가 더 크면 가로 방향으로 3칸 너비(30)만큼 이동
        if (diffX > diffY)
        {
            transform.Translate(Vector3.right * dirX * tileSize * 3);
        }
        else if (diffX < diffY)
        {
            transform.Translate(Vector3.up * dirY * tileSize * 3);
        }
    }
}
