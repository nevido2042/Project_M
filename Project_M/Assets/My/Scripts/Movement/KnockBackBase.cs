using System.Collections;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 넉백 기능을 제공하는 베이스 추상 클래스
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class KnockBackBase : MonoBehaviour, IKnockbackable
    {
        [Header("기본 넉백 설정")]
        [SerializeField] protected float defaultForce = 5f;
        [SerializeField] protected float defaultDuration = 0.2f;

        protected Rigidbody2D rb;
        protected Move move;
        protected Flip flip; // Flip 컴포넌트 참조 추가

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            move = GetComponent<Move>();
            flip = GetComponent<Flip>();
        }

        protected virtual void OnEnable()
        {
            // HealthBase 컴포넌트를 찾아 이벤트를 구독합니다.
            var damageable = GetComponent<HealthBase>();
            if (damageable != null)
            {
                damageable.OnDamaged += HandleDamaged;
            }
        }

        protected virtual void OnDisable()
        {
            // 구독 해제
            var damageable = GetComponent<HealthBase>();
            if (damageable != null)
            {
                damageable.OnDamaged -= HandleDamaged;
            }
        }

        private void HandleDamaged(DamageData data)
        {
            // 넉백 힘이 0보다 클 경우에만 실행
            if (data.knockbackForce > 0)
            {
                Vector2 dir = GetKnockBackDirection(data);
                ApplyKnockBack(dir, data.knockbackForce, data.knockbackDuration);
            }
        }

        /// <summary>
        /// 전달된 데미지 데이터를 바탕으로 넉백 방향을 결정합니다.
        /// 하위 클래스(EnemyKnockBack 등)에서 특수한 방향 계산이 필요할 경우 오버라이드합니다.
        /// </summary>
        protected virtual Vector2 GetKnockBackDirection(DamageData data)
        {
            if (data.sourcePos.HasValue)
            {
                return ((Vector2)transform.position - data.sourcePos.Value).normalized;
            }
            return Vector2.zero;
        }

        public virtual void ApplyKnockBack(Vector2 direction)
        {
            ApplyKnockBack(direction, defaultForce, defaultDuration);
        }

        public virtual void ApplyKnockBack(Vector2 direction, float force, float duration)
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(KnockBackRoutine(direction, force, duration));
            }
        }

        protected virtual IEnumerator KnockBackRoutine(Vector2 direction, float force, float duration)
        {
            OnKnockBackStart();

            // 넉백 동안 Flip 및 Move 비활성화
            if (flip != null) flip.enabled = false;
            if (move != null) move.enabled = false;

            rb.linearVelocity = direction.normalized * force;

            yield return new WaitForSeconds(duration);

            rb.linearVelocity = Vector2.zero;

            // 다시 활성화
            if (move != null) move.enabled = true;
            if (flip != null) flip.enabled = true;

            OnKnockBackEnd();
        }

        /// <summary>
        /// 넉백이 시작될 때 실행될 추가 로직 (하위 클래스에서 오버라이드 가능)
        /// </summary>
        protected virtual void OnKnockBackStart() { }

        /// <summary>
        /// 넉백이 종료될 때 실행될 추가 로직 (하위 클래스에서 오버라이드 가능)
        /// </summary>
        protected virtual void OnKnockBackEnd() { }
    }
}
