using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 효과음 종류 정의
    /// </summary>
    public enum SfxType
    {
        None,
        Hit,
        Die,
        LevelUp,
        Collect,
        ButtonClick,
        Lose
    }

    /// <summary>
    /// 배경음 종류 정의
    /// </summary>
    public enum BgmType
    {
        None,
        Main,
        InGame
    }

    /// <summary>
    /// 사운드 리소스 매핑 데이터 에셋
    /// </summary>
    [CreateAssetMenu(fileName = "AudioData", menuName = "Hero/Audio Data")]
    public class AudioData : ScriptableObject
    {
        [Serializable]
        public struct SfxMapping
        {
            public SfxType type;
            public AudioClip clip;
        }

        [Serializable]
        public struct BgmMapping
        {
            public BgmType type;
            public AudioClip clip;
        }

        [Header("효과음 리스트")]
        public List<SfxMapping> sfxList;

        [Header("배경음 리스트")]
        public List<BgmMapping> bgmList;

        private Dictionary<SfxType, AudioClip> sfxCache;
        private Dictionary<BgmType, AudioClip> bgmCache;

        /// <summary>
        /// 데이터를 딕셔너리로 캐싱하여 빠른 검색을 지원합니다.
        /// </summary>
        public void Init()
        {
            sfxCache = new Dictionary<SfxType, AudioClip>();
            foreach (var mapping in sfxList)
            {
                if (mapping.type != SfxType.None && !sfxCache.ContainsKey(mapping.type))
                    sfxCache.Add(mapping.type, mapping.clip);
            }

            bgmCache = new Dictionary<BgmType, AudioClip>();
            foreach (var mapping in bgmList)
            {
                if (mapping.type != BgmType.None && !bgmCache.ContainsKey(mapping.type))
                    bgmCache.Add(mapping.type, mapping.clip);
            }
        }

        public AudioClip GetSfx(SfxType type)
        {
            if (sfxCache == null) Init();
            return sfxCache.TryGetValue(type, out var clip) ? clip : null;
        }

        public AudioClip GetBgm(BgmType type)
        {
            if (bgmCache == null) Init();
            return bgmCache.TryGetValue(type, out var clip) ? clip : null;
        }
    }
}
