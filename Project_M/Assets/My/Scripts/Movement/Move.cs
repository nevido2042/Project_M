using UnityEngine;

namespace Hero
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("speed")] private float m_Speed = 5f;
        public float Speed { get => m_Speed; set => m_Speed = value; }

        private Rigidbody2D m_Rb;
        public Vector2 Velocity { get; set; }

        private void Awake()
        {
            m_Rb = GetComponent<Rigidbody2D>();
        }

        // 인스펙터에서 스크립트가 추가되거나 Reset 버튼을 눌렀을 때 실행됩니다.
        private void Reset()
        {
            m_Rb = GetComponent<Rigidbody2D>();
            if (m_Rb != null)
            {
                m_Rb.gravityScale = 0f;      // 중력을 0으로 설정
                m_Rb.freezeRotation = true; // 캐릭터의 굴러다님 방지
            }
        }

        private void FixedUpdate()
        {
            // 입력받은 벡터와 속도를 곱하여 물리적 속도로 적용 (유니티 6/2023.3+ 권장 방식)
            m_Rb.linearVelocity = Velocity * m_Speed;
        }
    }
}
