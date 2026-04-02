using UnityEngine;

namespace Hero
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;

        private Rigidbody2D rb;
        public Vector2 Velocity { get; set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // 인스펙터에서 스크립트가 추가되거나 Reset 버튼을 눌렀을 때 실행됩니다.
        private void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;      // 중력을 0으로 설정
                rb.freezeRotation = true; // 캐릭터의 굴러다님 방지
            }
        }

        private void FixedUpdate()
        {
            // 입력받은 벡터와 속도를 곱하여 물리적 속도로 적용 (유니티 6/2023.3+ 권장 방식)
            rb.linearVelocity = Velocity * speed;
        }
    }
}
