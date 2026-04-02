using UnityEngine;

/// <summary>
/// 카메라가 타겟(플레이어)을 부드럽게 추적하도록 제어하는 컴포넌트
/// </summary>
public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;           // 추적 대상 (Player)
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f); // 카메라 오프셋
    [SerializeField] private float smoothSpeed = 5f; // 부드러움 정도 (높을수록 빠름)

    private void LateUpdate()
    {
        if (target == null)
        {
            // 플레이어를 자동으로 찾으려 시도
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                return;
            }
        }

        // 목표 위치 계산 (Z축 오프셋 유지)
        Vector3 desiredPosition = target.position + offset;
        
        // 현재 위치에서 목표 위치로 선형 보간 (Lerp)
        // Time.deltaTime을 사용하여 일정한 속도로 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // 카메라 위치 업데이트
        transform.position = smoothedPosition;
    }
}
