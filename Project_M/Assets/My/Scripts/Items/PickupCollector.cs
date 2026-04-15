using System.Collections.Generic;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 주변의 아이템(경험치 등)을 탐지하고 플레이어에게 끌어당기는 자석 기능을 담당하는 클래스
    /// </summary>
    public class PickupCollector : MonoBehaviour
    {
        [Header("설정")]
        [SerializeField] private float pickupRadius = 3f;      // 아이템 탐지 범위
        [SerializeField] private float pullSpeed = 10f;       // 아이템을 끌어당기는 속도
        [SerializeField] private LayerMask itemLayer;         // 아이템 레이어

        public float PickupRadius
        {
            get => pickupRadius;
            set => pickupRadius = value;
        }

        [SerializeField] private float scanInterval = 0.2f;   // 아이템 탐지 주기 (단위: 초)
        [SerializeField] private int maxScanResults = 32;     // 한 번에 탐지할 최대 아이템 수

        private float scanTimer;
        private Collider2D[] scanResults;
        private List<Transform> activeItems = new List<Transform>();
        private ContactFilter2D filter;

        private void Awake()
        {
            scanResults = new Collider2D[maxScanResults];
            
            filter = new ContactFilter2D();
            filter.SetLayerMask(itemLayer);
            filter.useTriggers = true;
        }

        private void Update()
        {
            UpdateScan();
            UpdatePulling();
        }

        private void UpdateScan()
        {
            scanTimer += Time.deltaTime;
            if (scanTimer >= scanInterval)
            {
                scanTimer = 0f;

                // 캐싱된 filter를 사용하여 주변 아이템을 탐지합니다.
                int count = Physics2D.OverlapCircle(transform.position, pickupRadius, filter, scanResults);

                for (int i = 0; i < count; i++)
                {
                    Transform t = scanResults[i].transform;
                    // 이미 추적 중인 아이템이 아니라면 추가
                    if (!activeItems.Contains(t))
                    {
                        activeItems.Add(t);
                    }
                }
            }
        }

        private void UpdatePulling()
        {
            Vector3 playerPos = transform.position;

            for (int i = activeItems.Count - 1; i >= 0; i--)
            {
                Transform item = activeItems[i];

                // 아이템이 파괴되었거나 비활성화(획득)된 경우 리스트에서 제거
                if (item == null || !item.gameObject.activeInHierarchy)
                {
                    activeItems.RemoveAt(i);
                    continue;
                }

                // 이동 처리
                item.position = Vector3.MoveTowards(item.position, playerPos, pullSpeed * Time.deltaTime);
                
                // (선택 사항) 거리가 너무 멀어진 경우 추적 중단 로직을 추가할 수 있습니다.
            }
        }

        private void OnDrawGizmosSelected()
        {
            // 에디터에서 범위 시각화
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, pickupRadius);
        }
    }
}
