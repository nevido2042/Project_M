using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 몬스터 전용 재배치 컴포넌트
    /// </summary>
    public class EnemyReposition : MonoBehaviour, IRepositionable
    {
        private Enemy enemy;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
        }

        /// <summary>
        /// 플레이어의 위치와 진행 방향을 계산하여 적을 앞쪽 구역에 재배치합니다.
        /// </summary>
        public void Reposition(Vector3 playerPos, Vector2 playerDir)
        {
            // 몬스터가 죽은 상태라면 재배치하지 않음 (사망 애니메이션 및 시체 유지)
            if (enemy != null && enemy.IsDead) return;

            // 재배치 거리 및 랜덤 오프셋 계산
            float range = 20f;
            Vector3 spawnPos = playerPos + (Vector3)(playerDir.normalized * range);
            spawnPos += new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);

            // 새로운 위치로 순간이동
            transform.position = spawnPos;
        }
    }
}
