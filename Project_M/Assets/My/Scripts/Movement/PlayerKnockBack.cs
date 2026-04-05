using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어 전용 넉백 컴포넌트
    /// </summary>
    public class PlayerKnockBack : KnockBackBase
    {
        // Player 특화 로직이 필요한 경우 여기에 작성 (예: 화면 흔들림)
        protected override void OnKnockBackStart()
        {
            base.OnKnockBackStart();
            // TODO: 카메라 흔들림 효과 추가 가능
        }
    }
}
