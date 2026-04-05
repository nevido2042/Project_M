using System.Collections;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 캐릭터가 데미지를 입었을 때 하얗게 깜빡이는 효과를 제어하는 컴포넌트
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class DamageFlash : MonoBehaviour
    {
        [Header("깜빡임 설정")]
        [SerializeField] private Color flashColor = Color.white;
        [SerializeField] private float flashDuration = 0.1f;

        private SpriteRenderer spriteRenderer;
        private Material originalMaterial;
        private Material flashMaterial;
        private Coroutine flashCoroutine;

        private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");
        private static readonly int FlashColorId = Shader.PropertyToID("_FlashColor");

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalMaterial = spriteRenderer.material;
            
            // 전용 쉐이더를 사용하는 새로운 머티리얼 인스턴스 생성
            Shader flashShader = Shader.Find("Hero/DamageFlash");
            if (flashShader != null)
            {
                flashMaterial = new Material(flashShader);
                flashMaterial.SetColor(FlashColorId, flashColor);
            }
            else
            {
                Debug.LogWarning("DamageFlash: 'Hero/DamageFlash' 쉐이더를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 하얀색 깜빡임 효과를 호출합니다.
        /// </summary>
        public void CallFlash()
        {
            if (flashMaterial == null) return;

            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);
            
            flashCoroutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            // 머티리얼을 플래시용으로 교체하고 양을 1로 설정
            spriteRenderer.material = flashMaterial;
            flashMaterial.SetFloat(FlashAmountId, 1f);

            yield return new WaitForSeconds(flashDuration);

            // 다시 원래대로 복구
            flashMaterial.SetFloat(FlashAmountId, 0f);
            spriteRenderer.material = originalMaterial;
            
            flashCoroutine = null;
        }

        private void OnDestroy()
        {
            // 메모리 누수 방지를 위해 동적 생성된 머티리얼 파괴
            if (flashMaterial != null)
            {
                Destroy(flashMaterial);
            }
        }
    }
}
