using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 적 전용 넉백 컴포넌트
    /// </summary>
    public class EnemyKnockBack : KnockBackBase
    {
        // Enemy 특화 로직이 필요한 경우 여기에 작성 (예: 피격 효과, 색상 변경)
        protected override void OnKnockBackStart()
        {
            base.OnKnockBackStart();
            // TODO: 피격 애니메이션 또는 파티클 발생 가능
        }
    }
}
