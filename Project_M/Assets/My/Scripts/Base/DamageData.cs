using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 데미지 전달을 위한 정보 구조체
    /// </summary>
    public struct DamageData
    {
        public float damage;            // 데미지 양
        public Vector2? sourcePos;     // 타격 지점 (넉백 방향 계산용)
        public float knockbackForce;    // 넉백 힘 (0이면 넉백 없음)
        public float knockbackDuration; // 넉백 지속 시간
        public GameObject attacker;     // 공격 주체 (필요 시 활용)

        public DamageData(float damage, Vector2? sourcePos = null, float knockbackForce = 0f, float knockbackDuration = 0.1f, GameObject attacker = null)
        {
            this.damage = damage;
            this.sourcePos = sourcePos;
            this.knockbackForce = knockbackForce;
            this.knockbackDuration = knockbackDuration;
            this.attacker = attacker;
        }
    }
}
