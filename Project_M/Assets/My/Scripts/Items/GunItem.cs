using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 플레이어에게 탄환을 제공하는 아이템
    /// </summary>
    public class GunItem : MonoBehaviour, IPoolable
    {
        [SerializeField] private int ammoAmount = 30; // 제공할 탄환 수
        private UnityEngine.Pool.IObjectPool<GunItem> pool;

        public void SetPool(UnityEngine.Pool.IObjectPool<GunItem> pool)
        {
            this.pool = pool;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Player player = collision.GetComponent<Player>();
                if (player != null)
                {
                    player.AddGunAmmo(ammoAmount);
                    
                    if (GameManager.Instance?.Audio != null)
                        GameManager.Instance.Audio.PlaySFX(SfxType.Collect);

                    Release();
                }
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
