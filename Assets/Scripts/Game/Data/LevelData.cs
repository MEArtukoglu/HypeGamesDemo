using HypeGames.Scripts.AI.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Game.Data
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    [System.Serializable]
    public struct LevelData
    {
        [SerializeField] private int m_LevelID;
        [SerializeField] private int m_MaxAICount;
        [SerializeField] private float m_AISpawnInterval;
        [SerializeField] private AISpawnArgs m_AISpawnArgs;

        public int LevelID => m_LevelID;
        public int MaxAICount => m_MaxAICount;
        public float AISpawnInterval => m_AISpawnInterval;
        public AISpawnArgs AISpawnArgs => m_AISpawnArgs;

        public LevelData(int LevelID, int MaxAICount, float AISpawnInterval, AISpawnArgs AISpawnArgs)
        {
            this.m_LevelID = LevelID;
            this.m_MaxAICount = MaxAICount;
            this.m_AISpawnInterval = AISpawnInterval;
            this.m_AISpawnArgs = AISpawnArgs;
        }

        public static LevelData NULL => new LevelData(-1, -1, -1, AISpawnArgs.NULL);

        public static implicit operator bool(LevelData levelData)
        {
            return (levelData.m_LevelID != -1 && levelData.m_MaxAICount != -1 && levelData.m_AISpawnInterval != -1 && levelData.AISpawnArgs != AISpawnArgs.NULL);
        }
    }
}