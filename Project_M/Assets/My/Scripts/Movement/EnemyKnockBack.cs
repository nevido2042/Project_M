using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 적 전용 넉백 컴포넌트
    /// </summary>
    public class EnemyKnockBack : KnockBackBase
    {
        // Enemy 특화 로직이 필요한 경우 여기에 작성 (예: 피격 효과, 색상 변경)

        protected override Vector2 GetKnockBackDirection(DamageData data)
        {
            // 부모의 기본 로직 (소스 위치 기반) 우선 실행
            Vector2 dir = base.GetKnockBackDirection(data);

            // 소스 위치가 없는 경우 플레이어 위치를 기준으로 반대 방향 계산 (폴백)
            if (dir == Vector2.zero && GameManager.Instance != null && GameManager.Instance.Player != null)
            {
                dir = (transform.position - GameManager.Instance.Player.transform.position).normalized;
            }

            return dir == Vector2.zero ? Vector2.up : dir;
        }

        protected override void OnKnockBackStart()
        {
            base.OnKnockBackStart();
        }
    }
}
