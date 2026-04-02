using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어의 체력, 경험치 및 레벨 상태를 관리하는 클래스
/// </summary>
public class Player : MonoBehaviour, IDamageable
{
    [Header("체력 설정")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("경험치 및 레벨 설정")]
    [SerializeField] private int level = 1;
    [SerializeField] private float currentExp = 0;
    [SerializeField] private float nextExp = 100f; // 목표 경험치

    [Header("데미지 설정")]
    [SerializeField] private float invincibilityDuration = 0.5f; // 무적 지속 시간
    private bool isInvincible = false;

    // 인터페이스 구현: 체력 정보
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsInvincible => isInvincible;

    // 경험치 정보 프로퍼티
    public float CurrentExp => currentExp;
    public float NextExp => nextExp;
    public int Level => level;

    private void Awake()
    {
        // 초기 체력 설정
        currentHealth = maxHealth;
    }

    /// <summary>
    /// 경험치를 획득합니다.
    /// </summary>
    /// <param name="amount">획득할 경험치 양</param>
    public void GetExp(float amount)
    {
        currentExp += amount;

        // 레벨업 조건 체크
        while (currentExp >= nextExp)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// 레벨업을 진행하고 경험치를 차감 및 목표 경험치를 갱신합니다.
    /// </summary>
    private void LevelUp()
    {
        currentExp -= nextExp;
        level++;

        // 레벨업 처리에 따른 목표 경험치 증가 (예: 20% 증가)
        nextExp = Mathf.Round(nextExp * 1.2f);
        
        Debug.Log($"레벨업! 현재 레벨: {level}, 다음 목표: {nextExp}");
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible || currentHealth <= 0) return;

        currentHealth -= damage;
        StartCoroutine(InvincibilityRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    public void Die()
    {
        Debug.Log("플레이어가 사망했습니다!");
    }
}
