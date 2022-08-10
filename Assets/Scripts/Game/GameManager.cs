using HypeGames.Scripts.Events;
using HypeGames.Scripts.Game.Data;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.Manager;
using HypeGames.Scripts.UI;
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

    [DefaultExecutionOrder(13)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] private bool Debug = false;
        [SerializeField] ObserverVariable<int> m_CurLevelID;
        public ObserverVariable<int> CurLevelID => m_CurLevelID;


        private void Awake()
        {
            if (Instance)
            {
                HypeExtensions.DebugEditor(new Exception("[LevelManager.Awake] Instance already exits!"));
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
            m_CurLevelID = new ObserverVariable<int>();
            m_CurLevelID.OnValueChanged += OnCurLevelIDChanged;
        }

        private void Start()
        {
            UIManager.Instance.GameCanvas.SwitchPanel(0);
            GameEvents.OnLevelFail.AddListener(OnLevelFail);
            GameEvents.OnLevelSuccess.AddListener(OnLevelSuccess);
        }

        public void LoadCurrentLevel()
        {
            LevelManager.Instance.InitializeLevel(m_CurLevelID.Value, this.OnLevelInitialized, this.OnLevelEnd, X => HypeExtensions.DebugEditor(X));
        }

        public void OnLevelInitialized(LevelData LevelData)
        {
            if (Debug)
                HypeExtensions.DebugEditor($"[GameManager.OnLevelInitialized] LevelID:{LevelData.LevelID}");
        }

        public void OnLevelEnd(LevelData LevelData)
        {
            if (Debug)
                HypeExtensions.DebugEditor($"[GameManager.OnLevelEnd] LevelID:{LevelData.LevelID}");
        }

        public void OnLevelFail(LevelData LevelData)
        {
            m_CurLevelID.Value = 0;
        }

        public void OnLevelSuccess(LevelData LevelData)
        {
            if (m_CurLevelID.Value >= LevelManager.Instance.LevelsSO.Levels.Count - 1)
                m_CurLevelID.Value = 0;
            else
                m_CurLevelID.Value++;
        }

        public void OnCurLevelIDChanged(int previousValue, int newValue)
        {
            UIManager.Instance.GameCanvas.SetCurrentLevelText(newValue);
        }
    }
}