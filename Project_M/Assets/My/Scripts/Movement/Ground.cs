using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 맵 타일 재배치를 담당하는 클래스 (무한 배경 루프)
    /// </summary>
    public class Ground : MonoBehaviour, IRepositionable
    {
        [SerializeField] private float tileSize = 20f; // 타일 한 칸의 크기

        /// <summary>
        /// 플레이어와의 거리에 따라 타일을 반대편으로 재배치합니다.
        /// (감시자 컴포넌트인 Reposition.cs 로부터 호출됩니다)
        /// </summary>
        public void Reposition(Vector3 playerPos, Vector2 playerDir)
        {
            Vector3 myPos = transform.position;

            // 플레이어와 타일 간의 상대 거리 계산
            float diffX = Mathf.Abs(playerPos.x - myPos.x);
            float diffY = Mathf.Abs(playerPos.y - myPos.y);

            // 플레이어의 위치에 따른 재배치 방향 결정 (어느 쪽으로 뛰어넘어야 할지)
            float dirX = playerPos.x > myPos.x ? 1 : -1;
            float dirY = playerPos.y > myPos.y ? 1 : -1;

            // X축 거리차이가 Y축보다 더 크면 가로 방향으로 30m(tileSize*3) 점프
            if (diffX > diffY)
            {
                transform.Translate(Vector3.right * dirX * tileSize * 3);
            }
            // Y축 거리차이가 더 크면 세로 방향으로 30m(tileSize*3) 점프
            else if (diffX < diffY)
            {
                transform.Translate(Vector3.up * dirY * tileSize * 3);
            }
        }
    }
}
