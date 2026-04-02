using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 화면 상에 고정된 HUD 또는 머리 위에서 체력을 렌더링하는 View
/// </summary>
public class HealthBarView : MonoBehaviour
{
    [Header("기본 설정")]
    [SerializeField] private Slider healthSlider;                     // 체력바 Slider

    [Header("추적 옵션 (머리 위 배치 시)")]
    [SerializeField] private bool followTarget = false;             // 타겟을 지연 추적할지 여부
    [SerializeField] private Vector3 offset = new Vector3(0, 1.2f, 0); // 머리 위 오프셋
    [SerializeField] private GameObject visualRoot;                    // 시각적 루트 오브젝트

    private Transform targetTransform;
    private HealthViewModel viewModel;

    /// <summary>
    /// 외부에서 모델과 타겟을 명시적으로 연결합니다.
    /// </summary>
    public void Initialize(IDamageable model, Transform target)
    {
        targetTransform = target;
        viewModel = new HealthViewModel(model);
    }

    private void Start()
    {
        // 명시적으로 설정되지 않은 경우, 플레이어를 자동으로 찾아 연결 (HUD 용도)
        if (viewModel == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                IDamageable model = player.GetComponent<IDamageable>();
                if (model != null)
                {
                    Initialize(model, player.transform);
                }
            }
        }
    }

    private void Update()
    {
        if (viewModel == null) return;

        // 1. UI 데이터 갱신 (ViewModel을 통해 가공된 데이터 읽기)
        if (healthSlider != null)
        {
            healthSlider.value = viewModel.HealthRatio;
        }

        // 2. 사망 또는 상태에 따른 시각적 처리
        if (visualRoot != null)
        {
            // HUD로 쓸 때는 항상 보이게 하거나, 조건부로 처리 가능
            bool shouldShow = !viewModel.IsDead;
            visualRoot.SetActive(shouldShow);
        }
    }

    private void LateUpdate()
    {
        // 3. 추적 모드일 때만 월드 좌표 동기화
        if (followTarget && targetTransform != null)
        {
            transform.position = targetTransform.position + offset;
        }
    }
}
