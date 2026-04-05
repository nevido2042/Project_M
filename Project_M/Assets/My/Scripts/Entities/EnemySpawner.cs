using System;
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
        
        public int KillCount { get; private set; }
        public event Action<int> OnKillCountChanged;

        private void Start()
        {
            // 스폰 코루틴 시작
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            // 게임 내내 반복
            while (true)
            {
                // GameManager와 플레이어가 존재할 때만 스폰
                if (GameManager.Instance != null && GameManager.Instance.Player != null)
                {
                    Spawn();
                }
                
                // 설정된 시간만큼 대기
                yield return new WaitForSeconds(spawnTime);
            }
        }

        private void Spawn()
        {
            // 1. GameManager를 통해 풀에서 적 오브젝트를 가져옴
            if (GameManager.Instance == null || GameManager.Instance.Pool == null) return;
            
            Enemy enemy = GameManager.Instance.Pool.GetEnemy();
            if (enemy == null) return;

            // 2. 랜덤한 방향 및 거리 계산 (원형 좌표계 활용)
            float angle = UnityEngine.Random.Range(0f, 360f);
            float distance = UnityEngine.Random.Range(minDistance, maxDistance);

            float rad = angle * Mathf.Deg2Rad;
            Vector3 spawnPos = new Vector3(
                Mathf.Cos(rad) * distance,
                Mathf.Sin(rad) * distance,
                0f
            );

            // 3. GameManager가 제공하는 플레이어 기준 상대적 위치로 설정
            Transform playerTsn = GameManager.Instance.Player.transform;
            enemy.transform.position = playerTsn.position + spawnPos;
        }

        /// <summary>
        /// 킬 수를 증가시키고 이벤트를 발생시킵니다.
        /// </summary>
        public void AddKill()
        {
            KillCount++;
            OnKillCountChanged?.Invoke(KillCount);
        }
    }
}
