using HypeGames.Scripts.AI.Data;
using HypeGames.Scripts.Events;
using HypeGames.Scripts.Game.Data;
using HypeGames.Scripts.Game.ScriptableObjects;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.Manager;
using HypeGames.Scripts.Player;
using HypeGames.Scripts.Player.Health;
using HypeGames.Scripts.Player.Health.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HypeGames.Scripts.Game
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>
    [DefaultExecutionOrder(11)]
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [SerializeField] private LevelsSO m_LevelsSO;
        private PlayerController m_Player;

        private LevelData m_ActiveLevel;

        public LevelsSO LevelsSO => m_LevelsSO;
        public LevelData ActiveLevel => m_ActiveLevel;

        private void Awake()
        {
            if(Instance)
            {
                HypeExtensions.DebugEditor(new Exception("[LevelManager.Awake] Instance already exits!"));
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
            m_LevelsSO = Resources.Load<LevelsSO>("LevelsSODefault");
            m_Player = FindObjectOfType<PlayerController>();
            m_Player.GetComponent<PlayerHealth>().OnPlayerDead.AddListener(OnPlayerDead);
        }

        public void InitializeLevel(int LevelID, UnityAction<LevelData> OnLevelInitialized = null, UnityAction<LevelData> OnLevelEnd = null, UnityAction<Exception> OnException = null)
        {
            if(m_LevelsSO.TryFindLevel(LevelID, out LevelData levelData))
            {
                AISpawnManager.Instance.RunLevel(levelData, this.OnAILevelReady, this.OnNoEnemyAliveLeft, OnException: X => HypeExtensions.DebugEditor(X));
            }
            else
            {
                OnException?.Invoke(new Exception($"[LevelManager.InitializeLevel] Failed to find level! [LevelID:{LevelID}]"));
            }
        }

        public void OnAILevelReady(AILevelData AILevelData)
        {
            m_ActiveLevel = AILevelData.LevelData;
            GameEvents.OnLevelBegin.Invoke(AILevelData.LevelData);
        }

        public void OnNoEnemyAliveLeft(AILevelData AILevelData)
        {
            GameEvents.OnLevelSuccess.Invoke(AILevelData.LevelData);
        }

        public void OnPlayerDead(PlayerHealth playerHealth, PlayerHealthDamageData[] damageDatas)
        {
            if(m_ActiveLevel)
            {
                GameEvents.OnLevelFail?.Invoke(m_ActiveLevel);
                m_ActiveLevel = LevelData.NULL;
            }
        }
    }
}