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
            flip = GetComponent<Flip>(); // Flip 컴포넌트 캐싱
        }

        public virtual void ApplyKnockBack(Vector2 direction)
        {
            ApplyKnockBack(direction, defaultForce, defaultDuration);
        }

        public virtual void ApplyKnockBack(Vector2 direction, float force, float duration)
        {
            StartCoroutine(KnockBackRoutine(direction, force, duration));
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
