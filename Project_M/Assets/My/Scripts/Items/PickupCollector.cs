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

        private List<Transform> itemsInRange = new List<Transform>();

        private void Update()
        {
            // 근처 아이템 탐색 (물리 엔진 최적화를 위해 주기를 조절할 수 있으나 여기서는 단순 구현)
            Collider2D[] hitItems = Physics2D.OverlapCircleAll(transform.position, pickupRadius, itemLayer);

            foreach (var hit in hitItems)
            {
                // 아이템을 플레이어 방향으로 이동시킴
                // 아이템 스크립트에 별도의 'Pull' 로직이 없어도 여기서 위치를 직접 수정 가능
                Vector3 targetPos = transform.position;
                hit.transform.position = Vector3.MoveTowards(hit.transform.position, targetPos, pullSpeed * Time.deltaTime);
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
