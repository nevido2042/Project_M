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

        private Vector2 direction;
        private Rigidbody2D rb;
        private UnityEngine.Pool.IObjectPool<Bullet> pool;
        private float timer;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void SetPool(UnityEngine.Pool.IObjectPool<Bullet> pool)
        {
            this.pool = pool;
        }

        public void Init(Vector2 dir, float dmg)
        {
            direction = dir.normalized;
            damage = dmg;
            timer = 0f;

            // 방향에 맞춰 회전 설정 (오프셋 포함)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

            // 물리 엔진을 이용한 속도 설정
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
                IDamageable target = collision.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage, transform.position);
                }
                Release();
            }
        }

        public void Release()
        {
            if (this == null) return;
            if (pool != null)
                pool.Release(this);
            else
                gameObject.SetActive(false);
        }
    }
}
