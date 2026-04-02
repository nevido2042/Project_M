using UnityEngine;

/// <summary>
/// 재배치가 가능한 객체들이 구현해야 할 인터페이스
/// </summary>
public interface IRepositionable
{
    /// <summary>
    /// 플레이어의 위치를 기반으로 하위 클래스에서 각자의 재배치 로직을 수행합니다.
    /// </summary>
    /// <param name="playerPos">현재 플레이어의 위치</param>
    void Reposition(Vector3 playerPos);
}
