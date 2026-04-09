using System.Collections;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 원거리 공격을 담당하는 컴포넌트 (무제한 탄환 사격 가능)
    /// </summary>
    public class RangedWeapon : MonoBehaviour
    {
        [Header("공격 설정")]
        [SerializeField] private float fireRate = 0.5f;     // 발사 간격
        [SerializeField] private float scanRange = 10f;    // 적 탐색 범위
        [SerializeField] private LayerMask enemyLayer;     // 적 레이어

        public float FireRate
        {
            get => fireRate;
            set
            {
                fireRate = Mathf.Max(0.01f, value); // 0 이하가 되지 않도록 방어 코드
                fireInterval = new WaitForSeconds(fireRate);
            }
        }

        private Coroutine fireCoroutine;
        private WaitForSeconds fireInterval;

        private void Awake()
        {
            fireInterval = new WaitForSeconds(fireRate);
        }

        private void OnEnable()
        {
            StopFireRoutine();
            fireCoroutine = StartCoroutine(FireRoutine());
        }

        private void OnDisable()
        {
            StopFireRoutine();
        }

        private void StopFireRoutine()
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
        }

        private IEnumerator FireRoutine()
        {
            // 발사 속도가 너무 빠를 경우를 대비한 안전 장치
            if (fireRate <= 0) yield break;

            while (true)
            {
                Fire();
                yield return fireInterval;
            }
        }

        private void Fire()
        {
            // 가장 가까운 적 탐색
            Transform target = GetNearestEnemy();
            if (target == null) return;

            // 탄환 생성 및 발사
            if (GameManager.Instance != null && GameManager.Instance.Pool != null)
            {
                Bullet bullet = GameManager.Instance.Pool.GetBullet();
                if (bullet != null)
                {
                    bullet.transform.position = transform.position;
                    Vector2 dir = (target.position - transform.position).normalized;
                    bullet.Init(dir);
                    
                    // 사운드 효과
                    if (GameManager.Instance.Audio != null)
                        GameManager.Instance.Audio.PlaySFX(SfxType.Fire);
                }
            }
        }

        private Transform GetNearestEnemy()
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, scanRange, enemyLayer);
            
            Transform nearest = null;
            float minDistance = float.MaxValue;

            foreach (var hit in hitEnemies)
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = hit.transform;
                }
            }

            return nearest;
        }
    }
}
