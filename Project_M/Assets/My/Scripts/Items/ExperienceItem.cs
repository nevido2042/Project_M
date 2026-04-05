using UnityEngine;
using UnityEngine.Pool;

namespace Hero
{
    /// <summary>
    /// 플레이어가 획득할 수 있는 경험치 아이템
    /// </summary>
    public class ExperienceItem : MonoBehaviour, IPoolable
    {
        [SerializeField] private float expValue = 10f; // 제공할 경험치 양
        private IObjectPool<ExperienceItem> pool;

        /// <summary>
        /// 자신이 속한 풀을 설정합니다.
        /// </summary>
        public void SetPool(IObjectPool<ExperienceItem> pool)
        {
            this.pool = pool;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 플레이어 태그 확인
            if (collision.CompareTag("Player"))
            {
                // Player 컴포넌트를 직접 찾기보다는 인터페이스나 태그 기반으로 성능 고려 가능
                // 여기서는 기존 Player.cs에 구현된 GetExp를 호출합니다.
                Player player = collision.GetComponent<Player>();
                if (player != null)
                {
                    player.GetExp(expValue);
                    
                    if (GameManager.Instance?.Audio != null)
                        GameManager.Instance.Audio.PlaySFX(SfxType.Collect);

                    Release(); // 풀로 반납
                }
            }
        }

        public void Release()
        {
            if (pool != null)
            {
                pool.Release(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
