using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 오브젝트를 Z축을 기준으로 지속적으로 회전시키는 컴포넌트
    /// </summary>
    public class Rotate : MonoBehaviour
    {
        [Header("회전 설정")]
        [SerializeField] private float rotateSpeed = 100f; // 초당 회전 각도 (도)

        private void Update()
        {
            // Z축을 기준으로 지정된 속도만큼 회전 (프레임 독립적)
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }
}
