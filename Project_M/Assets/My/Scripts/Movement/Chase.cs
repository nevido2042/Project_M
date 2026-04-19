using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 대상을 추적하여 이동 방향을 결정하는 AI 컴포넌트 (몬스터용)
    /// </summary>
    [RequireComponent(typeof(Move))]
    public class Chase : MonoBehaviour
    {
        [Header("추적 설정")]
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("target")] private Transform m_Target; // 추적 대상

        private Move m_Move;

        private void Awake()
        {
            m_Move = GetComponent<Move>();
        }

        private void Start()
        {
            // 타겟이 지정되지 않았다면 "Player" 태그를 가진 오브젝트를 자동으로 찾음
            if (m_Target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    m_Target = player.transform;
                }
            }
        }

        private void FixedUpdate()
        {
            // 타겟이나 Move 컴포넌트가 없으면 작동 중지
            if (m_Target == null || m_Move == null)
            {
                if (m_Move != null) m_Move.Velocity = Vector2.zero;
                return;
            }

            // 플레이어 방향 벡터 계산
            Vector2 direction = (Vector2)m_Target.position - (Vector2)transform.position;

            // 방향을 정규화하여 Move 컴포넌트에 전달 (실제 물리 이동은 Move에서 처리)
            m_Move.Velocity = direction.normalized;
        }

        /// <summary>
        /// 추적 대상을 수동으로 설정합니다.
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            m_Target = newTarget;
        }
    }
}
