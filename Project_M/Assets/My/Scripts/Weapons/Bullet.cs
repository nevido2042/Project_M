using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 원거리 무기에서 발사되는 탄환 클래스
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float damage = 5f;
        [SerializeField] private float lifeTime = 5f;
        [SerializeField] private float rotationOffset = -90f; // 스프라이트 머리가 위쪽(Y+)일 경우 -90
        [SerializeField] private float knockbackForce = 5f;   // 원거리 탄환의 넉백 힘

        private Vector2 direction;
        private Rigidbody2D rb;
        private UnityEngine.Pool.IObjectPool<Bullet> pool;
        private float timer;
        private bool isReleased; // 중복 반납 방지 플래그

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void SetPool(UnityEngine.Pool.IObjectPool<Bullet> pool)
        {
            this.pool = pool;
        }

        public void Init(Vector2 dir, float damage, Sprite sprite)
        {
            direction = dir.normalized;
            this.damage = damage;
            
            if (spriteRenderer != null)
                spriteRenderer.sprite = sprite;
                
            timer = 0f;
            isReleased = false;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

            if (rb != null)
            {
                rb.linearVelocity = direction * speed;
            }
        }

        private void Update()
        {
            // 이제 이동은 물리 엔진(Rigidbody2D)이 담당합니다.

            // 생존 시간 체크
            timer += Time.deltaTime;
            if (timer >= lifeTime)
            {
                Release();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                HealthBase target = collision.GetComponent<HealthBase>();
                if (target != null)
                {
                    // 원거리 무기인 총알은 넉백을 적용함
                    DamageData data = new DamageData(damage, transform.position, knockbackForce);
                    target.TakeDamage(data);
                }
                Release();
            }
        }

        public void Release()
        {
            if (isReleased || this == null) return;
            isReleased = true;

            if (pool != null)
                pool.Release(this);
            else
                gameObject.SetActive(false);
        }
    }
}
