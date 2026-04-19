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
        private SpriteRenderer m_SpriteRenderer;
        private Rigidbody2D m_Rb;

        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Rb = GetComponent<Rigidbody2D>();
        }

        private void LateUpdate()
        {
            // 물리 연산이 끝난 후(LateUpdate) 속도를 체크하여 방향을 결정합니다.
            // 속도가 0이 아닐 때만 방향을 업데이트하여 정지 시 마지막 방향을 유지합니다.
            if (m_Rb != null && m_Rb.linearVelocity.x != 0)
            {
                m_SpriteRenderer.flipX = m_Rb.linearVelocity.x < 0;
            }
        }
    }
}
