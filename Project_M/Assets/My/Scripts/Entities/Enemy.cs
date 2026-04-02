using UnityEngine;

/// <summary>
/// 몬스터의 전체적인 행동을 관리하는 클래스
/// </summary>
public class Enemy : MonoBehaviour, IRepositionable, IDamageable
{
    [Header("능력치 설정")]
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float damageAmount = 10f; // 접촉 시 데미지 양
    private float currentHealth;

    private static Rigidbody2D playerRb;
    private static Transform playerTransform;

    // 인터페이스 구현: 체력 및 무적 정보
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsInvincible => false;

    private void Awake()
    {
        // 초기 체력 설정
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // 정적 변수에 플레이어 정보 캐싱 (효율적 참조)
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                playerRb = player.GetComponent<Rigidbody2D>();
            }
        }
    }

    /// <summary>
    /// 플레이어의 영역(Area)을 벗어날 때 재배치를 수행합니다.
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // "Area" 태그를 가진 트리거를 벗어날 때 실행
        if (!collision.CompareTag("Area")) return;

        if (playerTransform != null)
        {
            // 인터페이스를 상속받은 자기 자신의 재배치 로직을 호출합니다.
            Reposition(playerTransform.position);
        }
    }

    /// <summary>
    /// 플레이어의 위치와 진행 방향을 계산하여 적을 앞쪽 구역에 재배치합니다.
    /// </summary>
    public void Reposition(Vector3 playerPos)
    {
        if (playerTransform == null) return;

        // 플레이어의 이동 방향(속도) 확인
        Vector2 playerDir = (playerRb != null) ? playerRb.linearVelocity : Vector2.zero;
        
        // 재배치 거리 및 랜덤 오프셋 계산
        float range = 20f;
        Vector3 spawnPos = playerPos + (Vector3)(playerDir.normalized * range);
        spawnPos += new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);

        // 새로운 위치로 순간이동
        transform.position = spawnPos;

        // 재배치될 때 체력도 다시 채워줌 (재활용할 경우)
        currentHealth = maxHealth;
        gameObject.SetActive(true); // 혹시 비활성화 상태였다면 켬
    }

    /// <summary>
    /// 몬스터에게 데미지를 입힙니다.
    /// </summary>
    /// <param name="damage">입힐 데미지 양</param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // TODO: 몬스터 사망 효과 등을 구현합니다.
        // 지금은 임시로 오브젝트를 비활성화합니다.
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 플레이어와 접촉 중일 때 데미지를 입힘
    /// </summary>
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 충돌 대상이 플레이어 태그를 가지고 있다면
        if (collision.gameObject.CompareTag("Player"))
        {
            // IDamageable 인터페이스를 찾아 데미지를 줍니다.
            // 무적 여부 등은 플레이어 스크립트 내부에서 처리됩니다.
            IDamageable hit = collision.gameObject.GetComponent<IDamageable>();
            if (hit != null)
            {
                hit.TakeDamage(damageAmount);
            }
        }
    }
}
