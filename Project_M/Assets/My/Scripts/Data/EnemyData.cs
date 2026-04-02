using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 몬스터의 공통 능력치 데이터를 관리하는 ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Header("기본 능력치")]
        [SerializeField] private float maxHealth = 10f;
        [SerializeField] private float damageAmount = 10f;
        [SerializeField] private float moveSpeed = 2f; // 이동 속도 추가

        public float MaxHealth => maxHealth;
        public float DamageAmount => damageAmount;
        public float MoveSpeed => moveSpeed;
    }
}
