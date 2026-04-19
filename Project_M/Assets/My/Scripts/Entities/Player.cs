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
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("level")] private int m_Level = 1;
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("currentExp")] private float m_CurrentExp = 0;
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("nextExp")] private float m_NextExp = 100f; // 목표 경험치

        // 핵심 컴포넌트 참조
        private PlayerHealth m_Health;
        private RangedWeapon m_RangedWeapon;
        private MeleeWeapon m_MeleeWeapon;

        // UI 및 상태 업데이트를 위한 이벤트 정의
        public event Action<float, float> OnExpChanged;    // (current, next)
        public event Action<int> OnLevelChanged;          // (level)

        // 체력 정보 프로퍼티 (기존 호환성을 유지하기 위해 PlayerHealth에서 가져옴)
        public float CurrentHealth => m_Health != null ? m_Health.CurrentHealth : 0f;
        public float MaxHealth => m_Health != null ? m_Health.MaxHealth : 0f;
        public bool IsInvincible => m_Health != null ? m_Health.IsInvincible : false;

        public event Action<float, float> OnHealthChanged
        {
            add => m_Health.OnHealthChanged += value;
            remove => m_Health.OnHealthChanged -= value;
        }

        // 경험치 정보 프로퍼티
        public float CurrentExp => m_CurrentExp;
        public float NextExp => m_NextExp;
        public int Level => m_Level;

        private void Awake()
        {
            m_Health = GetComponent<PlayerHealth>();
            
            // 무기 시스템 자동 검색 및 할당
            m_RangedWeapon = GetComponentInChildren<RangedWeapon>();
            m_MeleeWeapon = GetComponentInChildren<MeleeWeapon>();
            
            // 초기 상태 알림
            OnExpChanged?.Invoke(m_CurrentExp, m_NextExp);
            OnLevelChanged?.Invoke(m_Level);
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
            m_CurrentExp += amount;

            // 레벨업 조건 체크
            while (m_CurrentExp >= m_NextExp)
            {
                LevelUp();
            }

            OnExpChanged?.Invoke(m_CurrentExp, m_NextExp);
        }

        /// <summary>
        /// 레벨업을 진행하고 경험치를 차감 및 목표 경험치를 갱신합니다.
        /// </summary>
        private void LevelUp()
        {
            m_CurrentExp -= m_NextExp;
            m_Level++;

            // 레벨업 처리에 따른 목표 경험치 증가 (예: 20% 증가)
            m_NextExp = Mathf.Round(m_NextExp * 1.2f);

            Debug.Log($"레벨업! 현재 레벨: {m_Level}, 다음 목표: {m_NextExp}");
            OnLevelChanged?.Invoke(m_Level);
        }

        /// <summary>
        /// 모든 무기 컴포넌트를 비활성화합니다.
        /// </summary>
        public void DeactivateWeapons()
        {
            if (m_RangedWeapon != null)
            {
                m_RangedWeapon.gameObject.SetActive(false);
            }

            if (m_MeleeWeapon != null)
            {
                m_MeleeWeapon.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
        }
    }
}
