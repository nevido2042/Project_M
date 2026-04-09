using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hero
{
    /// <summary>
    /// 탄환의 티어별 정보(데미지, 스프라이트)를 관리하는 데이터 클래스
    /// </summary>
    [CreateAssetMenu(fileName = "BulletData", menuName = "Data/BulletData")]
    public class BulletData : ScriptableObject
    {
        [Serializable]
        public struct BulletLevel
        {
            public float damage;          // 해당 티어의 공격력
            public Sprite bulletSprite;   // 해당 티어의 투사체 이미지
        }

        [Header("티어 설정")]
        public List<BulletLevel> levels = new List<BulletLevel>();

        /// <summary>
        /// 특정 티어의 데이터를 가져옵니다. 범위 초과 시 마지막 티어를 반환합니다.
        /// </summary>
        public BulletLevel GetLevelData(int index)
        {
            if (levels == null || levels.Count == 0) return default;
            int safeIndex = Mathf.Clamp(index, 0, levels.Count - 1);
            return levels[safeIndex];
        }

        public int MaxLevel => levels != null ? levels.Count : 0;
    }
}
