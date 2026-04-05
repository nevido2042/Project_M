using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 몬스터 전용 체력 관리 컴포넌트
    /// </summary>
    public class EnemyHealth : HealthBase
    {
        [Header("몬스터 데이터")]
        [SerializeField] private EnemyData data;

        public override bool IsInvincible => false;

        protected override void Awake()
        {
            // 부모 Awake가 maxHealth로 currentHealth를 초기화하기 전에
            // 데이터로부터 maxHealth를 먼저 설정함
            if (data != null)
            {
                maxHealth = data.MaxHealth;
            }
            base.Awake();
        }

        private void OnEnable()
        {
            // 풀에서 재활용될 때 체력 회복
            if (data != null)
            {
                maxHealth = data.MaxHealth;
                currentHealth = maxHealth;
            }
        }

        protected override void ApplyKnockBackEffect(Vector2? damageSourcePos)
        {
            // 몬스터 특유의 넉백 방향 보정 로직
            Vector2 dir;
            if (damageSourcePos.HasValue)
            {
                dir = ((Vector2)transform.position - damageSourcePos.Value).normalized;
            }
            else if (GameManager.Instance != null && GameManager.Instance.Player != null)
            {
                // 소스 위치가 없을 경우 플레이어 위치를 기준으로 반대 방향 계산
                dir = (transform.position - GameManager.Instance.Player.transform.position).normalized;
            }
            else
            {
                dir = Vector2.zero;
            }

            if (dir != Vector2.zero && knockbackable != null)
            {
                knockbackable.ApplyKnockBack(dir);
            }
        }
    }
}
