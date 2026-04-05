using System.Collections;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 오브젝트가 활성화된 후 일정 시간이 지나면 자동으로 풀로 반납되게 하는 컴포넌트
    /// (IPoolable 인터페이스를 구현한 모든 오브젝트에 적용 가능)
    /// </summary>
    public class LifeTime : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 10f; // 유지 시간 (단위: 초)
        
        private IPoolable poolable;
        private Coroutine expirationCoroutine;

        private void Awake()
        {
            // IPoolable 인터페이스를 구현한 컴포넌트를 찾습니다.
            poolable = GetComponent<IPoolable>();
        }

        private void OnEnable()
        {
            // 오브젝트가 활성화(스폰)될 때마다 타이머를 시작합니다.
            if (expirationCoroutine != null)
            {
                StopCoroutine(expirationCoroutine);
            }
            expirationCoroutine = StartCoroutine(ExpirationRoutine());
        }

        private void OnDisable()
        {
            // 오브젝트가 비활성화되면 실행 중인 코루틴을 정리합니다.
            if (expirationCoroutine != null)
            {
                StopCoroutine(expirationCoroutine);
                expirationCoroutine = null;
            }
        }

        private IEnumerator ExpirationRoutine()
        {
            // 설정된 시간만큼 대기
            yield return new WaitForSeconds(lifeTime);

            // 시간이 다 되면 풀로 반납
            if (poolable != null)
            {
                poolable.Release();
            }
        }
    }
}
