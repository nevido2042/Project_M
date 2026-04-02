using UnityEngine;

namespace Hero
{
    /// <summary>
    /// Rigidbody2D의 속도에 따라 SpriteRenderer의 flipX를 자동으로 제어하는 컴포넌트입니다.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Flip : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void LateUpdate()
        {
            // 물리 연산이 끝난 후(LateUpdate) 속도를 체크하여 방향을 결정합니다.
            // 속도가 0이 아닐 때만 방향을 업데이트하여 정지 시 마지막 방향을 유지합니다.
            if (rb != null && rb.linearVelocity.x != 0)
            {
                spriteRenderer.flipX = rb.linearVelocity.x < 0;
            }
        }
    }
}
