using System.Collections;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어 주변의 랜덤한 위치에 몬스터를 스폰하는 클래스
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("스폰 설정")]
        [SerializeField] private float spawnTime = 1.0f; // 스폰 시간 간격
        [SerializeField] private float minDistance = 10f; // 최소 스폰 거리 (화면 밖)
        [SerializeField] private float maxDistance = 15f; // 최대 스폰 거리

        private Transform playerTransform;

        private void Start()
        {
            // 플레이어 태그로 위치 추적 시작
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }

            // 스폰 코루틴 시작
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            // 게임 내내 반복
            while (true)
            {
                if (playerTransform != null)
                {
                    Spawn();
                }
                
                // 설정된 시간만큼 대기
                yield return new WaitForSeconds(spawnTime);
            }
        }

        private void Spawn()
        {
            // 1. PoolManager를 통해 풀에서 적 오브젝트를 가져옴
            Enemy enemy = PoolManager.Instance.GetEnemy();
            if (enemy == null) return;

            // 2. 랜덤한 방향 및 거리 계산 (원형 좌표계 활용)
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(minDistance, maxDistance);

            float rad = angle * Mathf.Deg2Rad;
            Vector3 spawnPos = new Vector3(
                Mathf.Cos(rad) * distance,
                Mathf.Sin(rad) * distance,
                0f
            );

            // 3. 플레이어 기준 상대적 위치로 설정
            enemy.transform.position = playerTransform.position + spawnPos;
        }
    }
}
