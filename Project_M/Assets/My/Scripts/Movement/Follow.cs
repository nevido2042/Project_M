using UnityEngine;

/// <summary>
/// 대상을 추적하여 이동하는 컴포넌트 (몬스터용)
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Follow : MonoBehaviour
{
    [Header("추적 설정")]
    [SerializeField] private float speed = 2f;          // 이동 속도
    [SerializeField] private Transform target;         // 추적 대상 (인스펙터에서 수동 할당 가능)

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // 타겟이 지정되지 않았다면 "Player" 태그를 가진 오브젝트를 자동으로 찾음
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        // 타겟이나 리지드바디가 없으면 작업을 중단
        if (target == null || rb == null) return;

        // 플레이어 방향 벡터 및 이동 속도 계산
        Vector2 direction = (Vector2)target.position - rb.position;
        Vector2 nextVec = direction.normalized * speed * Time.fixedDeltaTime;

        // 물리 엔진을 이용해 위치 이동 (관성 무시, 위치 직접 갱신)
        rb.MovePosition(rb.position + nextVec);

        // 시선 방향 전환 (Flip)
        if (direction.x != 0)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }
}
