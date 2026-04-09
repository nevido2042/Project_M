using System;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 체력 데이터를 UI가 사용하기 쉬운 형태로 가공하는 ViewModel
    /// </summary>
    public class HealthViewModel : IDisposable
    {
        private readonly HealthBase model;
        
        // 데이터 변경 알림 이벤트
        public event Action<float> OnHealthRatioChanged;

        public HealthViewModel(HealthBase model)
        {
            this.model = model;
            if (this.model != null)
            {
                this.model.OnHealthChanged += HandleHealthChanged;
            }
        }

        private void HandleHealthChanged(float current, float max)
        {
            float ratio = max > 0 ? Mathf.Clamp01(current / max) : 0;
            OnHealthRatioChanged?.Invoke(ratio);
        }

        /// <summary>
        /// 현재 체력 비율 (0 ~ 1)을 반환합니다.
        /// </summary>
        public float HealthRatio
        {
            get
            {
                if (model == null || model.MaxHealth <= 0) return 0f;
                return Mathf.Clamp01(model.CurrentHealth / model.MaxHealth);
            }
        }

        /// <summary>
        /// 대상이 사망했는지 확인합니다.
        /// </summary>
        public bool IsDead => model != null && model.CurrentHealth <= 0;

        public void Dispose()
        {
            if (model != null)
            {
                model.OnHealthChanged -= HandleHealthChanged;
            }
        }
    }
}

