using System;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어의 경험치 및 레벨 상태를 관리하는 클래스 (체력은 PlayerHealth에서 담당)
    /// </summary>
    public class Player : MonoBehaviour
    {
        [Header("경험치 및 레벨 설정")]
        [SerializeField] private int level = 1;
        [SerializeField] private float currentExp = 0;
        [SerializeField] private float nextExp = 100f; // 목표 경험치

        // 핵심 컴포넌트 참조
        private PlayerHealth health;
        private RangedWeapon rangedWeapon;
        private MeleeWeapon meleeWeapon;

        // UI 및 상태 업데이트를 위한 이벤트 정의
        public event Action<float, float> OnExpChanged;    // (current, next)
        public event Action<int> OnLevelChanged;          // (level)

        // 체력 정보 프로퍼티 (기존 호환성을 유지하기 위해 PlayerHealth에서 가져옴)
        public float CurrentHealth => health != null ? health.CurrentHealth : 0f;
        public float MaxHealth => health != null ? health.MaxHealth : 0f;
        public bool IsInvincible => health != null ? health.IsInvincible : false;

        public event Action<float, float> OnHealthChanged
        {
            add => health.OnHealthChanged += value;
            remove => health.OnHealthChanged -= value;
        }

        // 경험치 정보 프로퍼티
        public float CurrentExp => currentExp;
        public float NextExp => nextExp;
        public int Level => level;

        private void Awake()
        {
            health = GetComponent<PlayerHealth>();
            
            // 무기 시스템 자동 검색 및 할당
            rangedWeapon = GetComponentInChildren<RangedWeapon>();
            meleeWeapon = GetComponentInChildren<MeleeWeapon>();
            
            // 초기 상태 알림
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

        /// <summary>
        /// 모든 무기 컴포넌트를 비활성화합니다.
        /// </summary>
        public void DeactivateWeapons()
        {
            if (rangedWeapon != null)
            {
                rangedWeapon.gameObject.SetActive(false);
            }

            if (meleeWeapon != null)
            {
                meleeWeapon.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
        }
    }
}
