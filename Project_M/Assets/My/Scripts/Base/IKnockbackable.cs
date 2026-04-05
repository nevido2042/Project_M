using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 넉백 효과를 받을 수 있는 오브젝트가 구현해야 하는 인터페이스
    /// </summary>
    public interface IKnockbackable
    {
        /// <summary>
        /// 캐릭터에게 넉백 효과를 적용합니다.
        /// </summary>
        /// <param name="direction">넉백 방향</param>
        void ApplyKnockBack(Vector2 direction);

        /// <summary>
        /// 외부에서 넉백 힘과 시간을 직접 지정하여 적용합니다.
        /// </summary>
        void ApplyKnockBack(Vector2 direction, float force, float duration);
    }
}
