using System;
using System.Collections;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어의 체력, 경험치 및 레벨 상태를 관리하는 클래스
    /// </summary>
    public class Player : MonoBehaviour, IDamageable
    {
        [Header("체력 설정")]
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        [Header("경험치 및 레벨 설정")]
        [SerializeField] private int level = 1;
        [SerializeField] private float currentExp = 0;
        [SerializeField] private float nextExp = 100f; // 목표 경험치

        [Header("데미지 설정")]
        [SerializeField] private float invincibilityDuration = 0.5f; // 무적 지속 시간
        private bool isInvincible = false;

        private IKnockbackable knockbackable;
        private DamageFlash damageFlash; // 데미지 깜빡임 추가

        // UI 및 상태 업데이트를 위한 이벤트 정의
        public event Action<float, float> OnHealthChanged; // (current, max)
        public event Action<float, float> OnExpChanged;    // (current, next)
        public event Action<int> OnLevelChanged;          // (level)

        // 인터페이스 구현: 체력 정보
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsInvincible => isInvincible;

        // 경험치 정보 프로퍼티
        public float CurrentExp => currentExp;
        public float NextExp => nextExp;
        public int Level => level;

        private void Awake()
        {
            knockbackable = GetComponent<IKnockbackable>();
            damageFlash = GetComponent<DamageFlash>();

            // 초기 체력 설정 및 이벤트 발생
            currentHealth = maxHealth;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            OnExpChanged?.Invoke(currentExp, nextExp);
            OnLevelChanged?.Invoke(level);
        }

        private void Update()
        {
            // 이동 입력 및 기타 업데이트 로직을 여기에 구현합니다.
        }

        /// <summary>
        /// 경험치를 획득합니다.
        /// </summary>
        /// <param name="amount">획득할 경험치 양</param>
        public void GetExp(float amount)
        {
            currentExp += amount;

            // 레벨업 조건 체크
            while (currentExp >= nextExp)
            {
                LevelUp();
            }

            OnExpChanged?.Invoke(currentExp, nextExp);
        }

        /// <summary>
        /// 레벨업을 진행하고 경험치를 차감 및 목표 경험치를 갱신합니다.
        /// </summary>
        private void LevelUp()
        {
            currentExp -= nextExp;
            level++;

            // 레벨업 처리에 따른 목표 경험치 증가 (예: 20% 증가)
            nextExp = Mathf.Round(nextExp * 1.2f);

            Debug.Log($"레벨업! 현재 레벨: {level}, 다음 목표: {nextExp}");
            OnLevelChanged?.Invoke(level);
        }

        public void TakeDamage(float damage, Vector2? damageSourcePos = null)
        {
            if (isInvincible || currentHealth <= 0) return;

            currentHealth -= damage;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            // 깜빡임 효과 실행
            if (damageFlash != null) damageFlash.CallFlash();

            // 넉백 적용
            if (knockbackable != null && damageSourcePos.HasValue)
            {
                Vector2 dir = ((Vector2)transform.position - damageSourcePos.Value).normalized;
                // 인터페이스를 통한 넉백 실행
                knockbackable.ApplyKnockBack(dir);
            }

            StartCoroutine(InvincibilityRoutine());

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private IEnumerator InvincibilityRoutine()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invincibilityDuration);
            isInvincible = false;
        }

        public void Die()
        {
            Debug.Log("플레이어가 사망했습니다!");
        }
    }
}
