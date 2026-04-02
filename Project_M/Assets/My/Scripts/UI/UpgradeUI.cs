using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Hero
{
    /// <summary>
    /// 강화 선택창의 전체 가시성과 랜덤 선택지를 관리하는 클래스
    /// </summary>
    public class UpgradeUI : MonoBehaviour
    {
        [Header("UI 요소")]
        [SerializeField] private GameObject uiPanel;          // 전체 UI 패널 (활성/비활성용)
        [SerializeField] private UpgradeButton[] buttons;    // UI 상의 3개 버튼들

        [Header("데이터")]
        [SerializeField] private List<UpgradeData> allUpgrades; // 사용 가능한 전체 강화 리스트

        private Player player;
        private VirtualJoystick[] joysticks; // 발견된 조이스틱들 저장용

        private void Awake()
        {
            // 기본적으로 UI는 비활성화 상태로 시작
            if (uiPanel != null) uiPanel.SetActive(false);
            
            // 모든 강화 데이터의 레벨 초기화 (에셋 데이터 보존 방지)
            foreach (var data in allUpgrades)
            {
                data.ResetLevel();
            }
        }

        private void Start()
        {
            // 시작 시 씬의 조이스틱들을 미리 찾아 캐싱 (성능 최적화)
            joysticks = Object.FindObjectsByType<VirtualJoystick>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        }

        /// <summary>
        /// 강화 선택창을 엽니다.
        /// </summary>
        public void Show(Player targetPlayer)
        {
            player = targetPlayer;
            
            // 0. 캐싱된 조이스틱 비활성화 (클릭 간섭 방지)
            if (joysticks != null)
            {
                foreach (var joy in joysticks)
                {
                    if (joy != null) joy.gameObject.SetActive(false);
                }
            }

            // 1. 아직 강화 가능한 항목들만 필터링 (최대 레벨 제외)
            var availableUpgrades = allUpgrades.Where(u => u.CanUpgrade).ToList();
            
            if (availableUpgrades.Count == 0)
            {
                Debug.LogWarning("더 이상 가능한 강화가 없습니다!");
                return;
            }

            // 2. 셔플 (무작위 섞기)
            var randomUpgrades = availableUpgrades.OrderBy(x => Random.value).Take(buttons.Length).ToList();

            // 3. 버튼에 데이터 주입 및 활성화
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < randomUpgrades.Count)
                {
                    buttons[i].gameObject.SetActive(true);
                    buttons[i].SetUpgrade(randomUpgrades[i], OnUpgradeSelected);
                }
                //else
                //{
                //    buttons[i].gameObject.SetActive(false); // 선택지가 모자라면 버튼 비활성
                //}
            }

            // 4. UI 및 게임 일시정지 처리
            uiPanel.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지
        }

        /// <summary>
        /// 강화 선택 시 실행되는 콜백 메서드
        /// </summary>
        private void OnUpgradeSelected(UpgradeData data)
        {
            // 실제 강화 적용
            data.ApplyUpgrade(player);
            
            // 5. UI 닫기 및 게임 재개
            uiPanel.SetActive(false);
            Time.timeScale = 1f;

            // 6. 조이스틱 복구
            if (joysticks != null)
            {
                foreach (var joy in joysticks)
                {
                    if (joy != null) joy.gameObject.SetActive(true);
                }
            }
            
            Debug.Log($"[Upgrade] {data.upgradeName} (Lv.{data.currentLevel})이(가) 적용되었습니다!");
        }
    }
}
