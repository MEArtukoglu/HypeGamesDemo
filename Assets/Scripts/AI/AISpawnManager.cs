using HypeGames.Scripts.AI;
using HypeGames.Scripts.AI.Data;
using HypeGames.Scripts.AI.Extensions;
using HypeGames.Scripts.AI.Health;
using HypeGames.Scripts.AI.Health.Data;
using HypeGames.Scripts.AI.ScriptableObjects;
using HypeGames.Scripts.Animators;
using HypeGames.Scripts.DesignPatterns.Singleton;
using HypeGames.Scripts.Events;
using HypeGames.Scripts.Game.Data;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.Player;
using HypeGames.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace HypeGames.Scripts.Manager
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>
    [DefaultExecutionOrder(12)]
    public class AISpawnManager : SingletonPattern<AISpawnManager>
    {
        public bool IsInitialized { private set; get; }
        private AISpawnManagerSO m_Data;
        public const int MAX_AI_COUNT = 50;

        private AIController[] m_SpawnedAIs;
        private SpawnPoints m_SpawnPoints;

        private AILevelData m_RunningLevelData;
        private PlayerController m_Player;
        private int m_DeadAICount;

        public AISpawnManagerSO Data => m_Data;
        public AIController[] SpawnedAIs => m_SpawnedAIs;
        public int DeadAICount => m_DeadAICount;

        private void Awake()
        {
            #region Initialize
            AISpawnManagerSO newData = Resources.Load<AISpawnManagerSO>("AISpawnManagerSODefault");
            if (newData.Equals(null))
            {
                HypeExtensions.DebugEditor(new Exception($"[AISpawnManager.Awake] Failed to find AISpawnManagerSO!\nPlease make sure it exits in resources folder!"));
                return;
            }
            m_Data = newData;
            #endregion
            m_Player = GameObject.FindObjectOfType<PlayerController>();
            m_SpawnedAIs = new AIController[MAX_AI_COUNT];
            m_RunningLevelData = AILevelData.NULL;
            if (!SpawnPoints.TryFindSpawnPoints(0, out m_SpawnPoints))
            {
                HypeExtensions.DebugEditor(new Exception($"[AISpawnManager.Awake] Failed to find SpawnPoints ID:{0}!"));
                return;
            }
            for (int i = 0; i < MAX_AI_COUNT; i++)
            {
                AIController newSpawnedAI = GameObject.Instantiate(m_Data.EnemyAIPrefab, transform, true);
                AIHealth newSpawnAIHealth = newSpawnedAI.gameObject.GetComponent<AIHealth>();
                newSpawnAIHealth.OnDead.AddListener(this.OnAIDead);
                newSpawnedAI.OnInitialized();
                m_SpawnedAIs[i] = newSpawnedAI;
            }
            GameEvents.OnLevelFail.AddListener(OnLevelFail);
            IsInitialized = true;
        }

        private void Update()
        {
            if (!IsInitialized)
                return;


            for (int i = 0; i < m_SpawnedAIs.Length; i++)
            {
                if (m_SpawnedAIs[i].CurTarget == true)
                    m_SpawnedAIs[i].OnUpdate(X => HypeExtensions.DebugEditor($"[AISpawnManager.m_SpawnedAIs({i}).OnUpdate] Exception: {X}"));
            }

            if (m_RunningLevelData == true)
            {
                float nextAISpawnTime = (float)m_RunningLevelData.LastAISpawnTime + (float)m_RunningLevelData.LevelData.AISpawnInterval;
                int totalSpawnedAICount = m_RunningLevelData.TotalSpawnedAICount;
                int maxAICount = m_RunningLevelData.LevelData.MaxAICount;
                List<AIController> spawnedAIs = m_RunningLevelData.SpawnedAIs;

                if (Time.time >= nextAISpawnTime && totalSpawnedAICount < maxAICount)
                {
                    totalSpawnedAICount++;
                    AIController aiToSpawn = m_SpawnedAIs[totalSpawnedAICount];
                    CharacterAnimator aiToSpawnAnimator = aiToSpawn.GetComponent<CharacterAnimator>();
                    aiToSpawnAnimator.OnReset();
                    aiToSpawn.gameObject.SetActive(true);
                    aiToSpawn.SetEnabled(new AITarget(true, m_Player.transform, m_SpawnPoints.GetNextPoint().position), m_RunningLevelData.LevelData.AISpawnArgs);
                    float lastAISpawnTime = Time.time;
                    spawnedAIs.Add(aiToSpawn);
                    m_RunningLevelData = new AILevelData(m_RunningLevelData.LevelData, lastAISpawnTime, spawnedAIs, m_RunningLevelData.OnAILevelReady, m_RunningLevelData.OnNoEnemyAliveLeft);
                }

                if (totalSpawnedAICount >= maxAICount)
                {
                    if (spawnedAIs.TrueForAll(X => X.gameObject.GetComponent<AIHealth>().IsDead))
                    {
                        m_RunningLevelData.OnNoEnemyAliveLeft?.Invoke(m_RunningLevelData);
                        m_RunningLevelData = AILevelData.NULL;
                    }
                }
            }
        }

        public void RunLevel(LevelData levelData, UnityAction<AILevelData> OnAILevelReady = null, UnityAction<AILevelData> OnNoEnemyAliveLeft = null, UnityAction<Exception> OnException = null, [CallerMemberName] string memberName = "")
        {
            if(!IsInitialized)
            {
                OnException?.Invoke(new Exception("[AISpawnManager.RunLevel] Manager was not initialized!"));
                return;
            }
            this.m_DeadAICount = 0;
            this.m_RunningLevelData = new AILevelData(levelData, 0, null, OnAILevelReady, OnNoEnemyAliveLeft);

            m_SpawnedAIs.ResetAll();
            m_RunningLevelData.OnAILevelReady?.Invoke(m_RunningLevelData);
        }

        public void OnAIDead(AIHealth AI, AIHealthDamageData[] damageDatas)
        {
            m_DeadAICount++;
        }

        public void OnLevelFail(LevelData levelData)
        {
            m_RunningLevelData = AILevelData.NULL;
        }
    }
}