using System;
using System.Collections;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어 전용 체력 관리 컴포넌트
    /// </summary>
    public class PlayerHealth : HealthBase
    {
        [Header("플레이어 특화 설정")]
        [SerializeField] private float invincibilityDuration = 0.5f;
        private bool isInvincible = false;
        private Animator animator;


        public override bool IsInvincible => isInvincible;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        public override void TakeDamage(DamageData data)
        {
            if (isInvincible || currentHealth <= 0) return;

            base.TakeDamage(data);

            if (currentHealth > 0)
            {
                StartCoroutine(InvincibilityRoutine());
            }
        }

        private IEnumerator InvincibilityRoutine()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invincibilityDuration);
            isInvincible = false;
        }

        public override void Die()
        {
            if (animator != null)
            {
                animator.SetBool("Dead", true);
            }
            
            // 이동 및 입력 비활성화
            var move = GetComponent<Move>();
            if (move != null)
            {
                move.Velocity = Vector2.zero;
                move.enabled = false;
            }

            var input = GetComponent<Hero.Input>();
            if (input != null)
            {
                input.enabled = false;
            }

            // 물리적 속도 즉시 정지
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            base.Die();

            // 게임 매니저를 통해 게임 오버 시퀀스 호출
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }

            Debug.Log("플레이어가 사망했습니다!");
        }

        public override void Heal(float amount)
        {
            base.Heal(amount);
        }
    }
}
