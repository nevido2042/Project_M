using System;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.SceneManagement;

namespace Hero
{
    /// <summary>
    /// 모든 핵심 시스템과 게임의 흐름(시작/종료)을 중앙에서 관리하는 퍼사드(Facade) 매니저
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = UnityEngine.Object.FindAnyObjectByType<GameManager>();
                    if (instance == null)
                    {
                        Debug.LogError("씬에 GameManager가 존재하지 않습니다!");
                    }
                }
                return instance;
            }
        }

        [Header("핵심 시스템 참조")]
        [SerializeField] private PoolManager pool;
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private UpgradeManager upgrade;
        [SerializeField] private Player player;
        [SerializeField] private AudioManager audioMgr;

        /* UI 시스템 참조는 이제 각 UI 컴포넌트(LobbyUI, PauseMenuUI 등)에서 이벤트를 구독하여 처리합니다. */

        // 게임 상태 관리 이벤트
        public event Action OnGameStart;
        public event Action OnGamePause;
        public event Action OnGameResume;
        public event Action OnGameOver;

        // 외부에서 접근할 수 있는 읽기 전용 프로퍼티
        public PoolManager Pool => pool;
        public EnemySpawner Spawner => spawner;
        public UpgradeManager Upgrade => upgrade;
        public Player Player => player;
        public AudioManager Audio => audioMgr;

        private void Update()
        {
            // [디버그 전용] L 키를 누르면 즉시 레벨업 (New Input System 기반)
            if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
            {
                if (player != null)
                {
                    float expNeeded = player.NextExp - player.CurrentExp;
                    player.GetExp(expNeeded);
                    Debug.Log("<color=yellow>[Cheat] 레벨업 치트 사용!</color>");
                }
            }
        }


private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void Start()
        {
            // [초기 상태: 로비]
            Time.timeScale = 0f;
            
            // UI 상태는 각 UI 클래스(LobbyUI 등)에서 이벤트 및 Start를 통해 초기화됩니다.

            // BGM 시작 (로비/메인 배경음)
            if (audioMgr != null) audioMgr.PlayBGM(BgmType.Main);
        }


        /// <summary>
        /// 실제 게임을 기동 (로비 -> 인게임 전환 시 호출)
        /// </summary>
        public void StartGame()
        {
            Time.timeScale = 1f;
            
            // 게임 시작 이벤트 호출 (UI 구성 요소들이 이를 구독하여 스스로를 켜고 끕니다)

            // 게임 시작 이벤트 호출 (조이스틱 등에서 구독 가능)
            OnGameStart?.Invoke();

            // 인게임 배경음으로 교체
            if (audioMgr != null) audioMgr.PlayBGM(BgmType.InGame);

            Debug.Log("[Game] 몬스터 정벌이 시작되었습니다!");
        }


        /// <summary>
        /// 게임 일시 정지 (플레이 중 호출)
        /// </summary>
        public void PauseGame()
        {
            Time.timeScale = 0f;
            
            // if (pauseMenuUI != null) pauseMenuUI.SetActive(true); // 이제 UI에서 이벤트를 구독하여 처리
            // if (pauseButton != null) pauseButton.SetActive(false); 

            OnGamePause?.Invoke();

            Debug.Log("[Game] 게임이 일시 정지되었습니다.");
        }

        /// <summary>
        /// 게임 재개 (PauseMenuUI에서 호출 권장)
        /// </summary>
        public void ResumeGame()
        {
            Time.timeScale = 1f;
            
            // OnGameResume 이벤트를 통해 각 UI가 처리

            OnGameResume?.Invoke();

            Debug.Log("[Game] 전투를 재개합니다.");
        }

        /// <summary>
        /// [레거시 지원] 인스펙터에서 직접 호출하던 경우를 위한 오버로드
        /// </summary>
        public void ResumeGame(GameObject legacyParam) => ResumeGame();

        /// <summary>
        /// 플레이어 사망 시 호출되는 게임 오버 로직
        /// </summary>
        public void GameOver()
        {
            // 약간의 지연 후 게임을 멈추고 메뉴를 띄움 (애니메이션 확인용)
            StartCoroutine(GameOverRoutine());
        }

        private System.Collections.IEnumerator GameOverRoutine()
        { 
            yield return new WaitForSecondsRealtime(1.0f);

            Time.timeScale = 0f;

            // 패배 효과음 재생 및 배경음 정지
            if (audioMgr != null)
            {
                audioMgr.StopBGM();
                audioMgr.PlaySFX(SfxType.Lose);
            }

            // 게임 종료 이벤트 호출

            // 게임 종료 이벤트 호출
            OnGameOver?.Invoke();

            Debug.Log("[Game] 게임 오버!");
        }


        /// <summary>
        /// 씬 리스타트
        /// </summary>
        public void RestartScene()
        {
            Time.timeScale = 1f;
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        /// <summary>
        /// 종료
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
