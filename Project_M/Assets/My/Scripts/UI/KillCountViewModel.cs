using System;

namespace Hero
{
    /// <summary>
    /// 킬 카운트 데이터를 UI에서 사용하기 위해 가공하는 ViewModel
    /// </summary>
    public class KillCountViewModel
    {
        private readonly EnemySpawner model;

        // 텍스트 변경 알림 이벤트
        public event Action<string> OnKillCountTextChanged;

        public KillCountViewModel(EnemySpawner model)
        {
            this.model = model;
            
            // 모델의 이벤트 구독
            if (this.model != null)
            {
                this.model.OnKillCountChanged += HandleKillCountChanged;
            }
        }

        /// <summary>
        /// 현재 킬 수의 문자열 표현
        /// </summary>
        public string KillCountText => model != null ? model.KillCount.ToString() : "0";

        private void HandleKillCountChanged(int currentKills)
        {
            // 모델의 데이터가 변경되면 가공하여 뷰에 알림
            OnKillCountTextChanged?.Invoke(currentKills.ToString());
        }

        /// <summary>
        /// 이벤트 구독 해제 (메모리 누수 방지)
        /// </summary>
        public void Dispose()
        {
            if (model != null)
            {
                model.OnKillCountChanged -= HandleKillCountChanged;
            }
        }
    }
}
