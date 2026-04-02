using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어의 근접 무기인 삽(Shovel)을 제어하는 클래스
    /// </summary>
    public class Shovel : MonoBehaviour
    {
        [Header("공격 설정")]
        [SerializeField] private float damage = 10f; // 삽의 기본 데미지

        /// <summary>
        /// 다른 콜라이더(Trigger)와 충돌했을 때 호출됩니다.
        /// </summary>
        /// <param name="collision">충돌한 콜라이더 정보</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 1. 충돌한 대상이 데미지를 입을 수 있는 인터페이스(IDamageable)를 가졌는지 확인
            IDamageable target = collision.GetComponent<IDamageable>();
            
            if (target != null)
            {
                // 2. 대상이 무적 상태가 아닐 경우에만 데미지 부여
                if (!target.IsInvincible)
                {
                    target.TakeDamage(damage);
                    
                    // 디버그용 로그 (필요 시 주석 처리)
                    Debug.Log($"{collision.name}에게 {damage}의 데미지를 입혔습니다.");
                }
            }
        }
    }
}
