using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어의 영역(Area)을 벗어나면 재배치를 수행하도록 명령하는 감시자 컴포넌트
    /// </summary>
    public class Reposition : MonoBehaviour
    {
        private IRepositionable repositionable;
        
        // 정적 변수를 사용하여 모든 Reposition 컴포넌트가 플레이어 정보를 공유 (성능 최적화)
        private static Transform playerTransform;
        private static Rigidbody2D playerRb;

        private void Awake()
        {
            // 같은 오브젝트 내의 재배치 행위자 참조
            repositionable = GetComponent<IRepositionable>();
        }

        private void Start()
        {
            // 플레이어 정보를 한 번만 찾아서 캐싱
            if (playerTransform == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    playerTransform = playerObj.transform;
                    playerRb = playerObj.GetComponent<Rigidbody2D>();
                }
            }
        }

        /// <summary>
        /// 플레이어의 영역(Area) 트리거를 벗어날 때 호출됩니다.
        /// </summary>
        private void OnTriggerExit2D(Collider2D collision)
        {
            // "Area" 태그를 가진 트리거(플레이어 주변 감지 영역)를 벗어날 때만 실행
            if (!collision.CompareTag("Area")) return;
            if (repositionable == null || playerTransform == null) return;

            // 인터페이스를 통해 구현된 고유의 재배치 로직 실행
            Vector2 playerDir = (playerRb != null) ? playerRb.linearVelocity : Vector2.zero;
            repositionable.Reposition(playerTransform.position, playerDir);
        }
    }
}
