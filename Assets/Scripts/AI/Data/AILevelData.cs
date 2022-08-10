using HypeGames.Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HypeGames.Scripts.AI.Data
{
    [System.Serializable]
    public struct AILevelData
    {
        [SerializeField] private LevelData m_LevelData;
        [SerializeField] private UnityAction<AILevelData> m_OnAILevelReady;
        [SerializeField] private UnityAction<AILevelData> m_OnNoEnemyAliveLeft;

        private float m_LastAISpawnTime;
        private List<AIController> m_SpawnedAIs;

        public LevelData LevelData => m_LevelData;
        public UnityAction<AILevelData> OnAILevelReady => m_OnAILevelReady;
        public UnityAction<AILevelData> OnNoEnemyAliveLeft => m_OnNoEnemyAliveLeft;

        public float LastAISpawnTime => m_LastAISpawnTime;
        public List<AIController> SpawnedAIs => m_SpawnedAIs;
        public int TotalSpawnedAICount => m_SpawnedAIs.Count;

        public AILevelData(LevelData LevelData, float LastAISpawnTime = -1, List<AIController> SpawnedAIs = null, UnityAction<AILevelData> OnAILevelReady = null, UnityAction<AILevelData> OnNoEnemyAliveLeft = null)
        {
            this.m_LevelData = LevelData;
            this.m_OnAILevelReady = OnAILevelReady;
            this.m_OnNoEnemyAliveLeft = OnNoEnemyAliveLeft;
            this.m_LastAISpawnTime = LastAISpawnTime;
            if (SpawnedAIs == null)
                this.m_SpawnedAIs = new List<AIController>();
            else
                this.m_SpawnedAIs = SpawnedAIs;
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return base.Equals(obj);

            if(obj.GetType().Equals(typeof(AILevelData)))
            {
                AILevelData aiLevelData = (AILevelData)obj;
                if (aiLevelData.m_LevelData == m_LevelData && aiLevelData.OnAILevelReady == m_OnAILevelReady && aiLevelData.OnNoEnemyAliveLeft == m_OnNoEnemyAliveLeft)
                    return true;
                else
                    return false;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public static AILevelData NULL => new AILevelData(LevelData.NULL, -1, null, null, null);

        public static implicit operator bool(AILevelData aiLevelData)
        {
            return (aiLevelData.m_LevelData != LevelData.NULL && aiLevelData.m_OnAILevelReady != null && aiLevelData.m_OnNoEnemyAliveLeft != null);
        }
    }
}