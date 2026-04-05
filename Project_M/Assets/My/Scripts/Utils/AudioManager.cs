using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 게임 내 모든 오디오 재생을 전역으로 관리하는 싱글톤 매니저
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("사운드 데이터 설정")]
        [SerializeField] private AudioData audioData;

        [Header("오디오 소스 설정")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("볼륨 설정")]
        [Range(0f, 1f)][SerializeField] private float bgmVolume = 1f;
        [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // 데이터 캐싱 초기화
            if (audioData != null) audioData.Init();

            // 소스가 없으면 자동 추가
            if (bgmSource == null) bgmSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();

            // BGM 설정
            bgmSource.loop = true;
            ApplyVolumes();
        }

        #region 재생 관련
        /// <summary>
        /// 특정 배경음을 재생합니다. (하나의 곡만 반복)
        /// </summary>
        public void PlayBGM(BgmType type)
        {
            if (audioData == null) return;
            var clip = audioData.GetBgm(type);
            if (clip == null) return;

            // 이미 재생 중이라면 중복 재생 방지
            if (bgmSource.isPlaying && bgmSource.clip == clip) return;

            bgmSource.clip = clip;
            bgmSource.Play();
        }

        /// <summary>
        /// 특정 효과음을 재생합니다. (다수 중첩 가능)
        /// </summary>
        public void PlaySFX(SfxType type)
        {
            if (audioData == null) return;
            var clip = audioData.GetSfx(type);
            if (clip != null)
                sfxSource.PlayOneShot(clip);
        }

        /// <summary>
        /// BGM 정지
        /// </summary>
        public void StopBGM() => bgmSource.Stop();
        #endregion

        #region 볼륨 제어
        public void SetBgmVolume(float volume)
        {
            bgmVolume = Mathf.Clamp01(volume);
            ApplyVolumes();
        }

        public void SetSfxVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            ApplyVolumes();
        }

        private void ApplyVolumes()
        {
            bgmSource.volume = bgmVolume;
            sfxSource.volume = sfxVolume;
        }
        #endregion
    }
}
