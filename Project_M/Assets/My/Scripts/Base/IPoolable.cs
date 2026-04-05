namespace Hero
{
    /// <summary>
    /// 오브젝트 풀링 시스템에서 관리되는 오브젝트를 위한 인터페이스
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// 오브젝트를 풀로 반납하거나 비활성화합니다.
        /// </summary>
        void Release();
    }
}
