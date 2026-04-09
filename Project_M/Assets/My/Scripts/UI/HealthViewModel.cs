using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 체력 데이터를 UI가 사용하기 쉬운 형태로 가공하는 ViewModel
    /// </summary>
    public class HealthViewModel
    {
        private readonly HealthBase model;

        public HealthViewModel(HealthBase model)
        {
            this.model = model;
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
    }
}
