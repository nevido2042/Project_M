using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 캐릭터 애니메이션 및 시각적 상태(Flip)를 제어하는 컴포넌트
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Anim : MonoBehaviour
    {
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

        private Animator animator;
        
        private Move movement;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            
            movement = GetComponent<Move>();
        }

    private void Update()
    {
        if (movement == null) return;

        // 이동 속도에 따라 애니메이터 파라미터 업데이트
        float currentSpeed = movement.Velocity.magnitude;
        animator.SetFloat(SpeedHash, currentSpeed);

            
        }
    }
}
