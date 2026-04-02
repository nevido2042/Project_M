using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 근접 무기(삽 등)를 플레이어 주변에 원형으로 배치하고 관리하는 클래스
    /// </summary>
    public class MeleeWeapon : MonoBehaviour
    {
        [Header("배치 설정")]
        [SerializeField] private GameObject weaponPrefab; // 배치할 무기 프리팹
        [Range(1, 20)]
        [SerializeField] private int count = 1;         // 무기 개수
        [SerializeField] private float radius = 2.0f;   // 플레이어 중심에서의 거리

        private bool isUpdatePending = false; // 에디터 지연 업데이트 중복 방지 플래그

        private void Start()
        {
            // 게임 시작 시 초기 배치 수행
            RebuildLayout();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // 이미 업데이트가 예약되어 있다면 중복 등록하지 않음
            if (isUpdatePending) return;

            isUpdatePending = true;

            // OnValidate 내에서 직접 Instantiate/Destroy를 호출하면 에러가 발생하므로 delayCall 사용
            UnityEditor.EditorApplication.delayCall += () =>
            {
                isUpdatePending = false;
                if (this == null) return;
                RebuildLayout();
            };
        }
#endif

        /// <summary>
        /// 현재 설정된 개수와 반경에 맞춰 무기들을 재배치합니다.
        /// </summary>
        public void RebuildLayout()
        {
            if (weaponPrefab == null) return;

            // 1. 기존의 모든 자식 무기 제거 (역순으로 접근하여 안전하게 삭제)
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (Application.isPlaying)
                    Destroy(child);
                else
                    DestroyImmediate(child);
            }

            // 2. 새로운 레이아웃 생성
            for (int i = 0; i < count; i++)
            {
                // 각 무기 사이의 각도 계산 (360도를 개수로 나눔)
                float angle = i * (360f / count);
                float radians = angle * Mathf.Deg2Rad;

                // 삼각함수로 좌표 계산
                float x = Mathf.Cos(radians) * radius;
                float y = Mathf.Sin(radians) * radius;

                // 무기 생성 및 위치 설정
                GameObject weapon = Instantiate(weaponPrefab, transform);
                weapon.transform.localPosition = new Vector3(x, y, 0);

                // 무기가 원의 바깥쪽을 향하도록 회전 설정
                // (기본 프리팹 방향에 따라 90도 혹은 180도 보정이 필요할 수 있음)
                weapon.transform.localRotation = Quaternion.Euler(0, 0, angle -90f);
            }
        }

        /// <summary>
        /// 무기 개수를 변경하고 배치를 갱신합니다. (레벨업 등에서 호출)
        /// </summary>
        /// <param name="newCount">새로운 무기 개수</param>
        public void SetWeaponCount(int newCount)
        {
            count = Mathf.Max(1, newCount);
            RebuildLayout();
        }
    }
}
