using HypeGames.Scripts.DesignPatterns.Singleton;
using HypeGames.Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HypeGames.Scripts.Events
{
    public class GameEvents : SingletonPattern<GameEvents>
    {
        [SerializeField] private UnityEvent<LevelData> m_OnLevelBegin;
        [SerializeField] private UnityEvent<LevelData> m_OnLevelFail;
        [SerializeField] private UnityEvent<LevelData> m_OnLevelSuccess;

        public static UnityEvent<LevelData> OnLevelBegin => Instance.m_OnLevelBegin;
        public static UnityEvent<LevelData> OnLevelFail => Instance.m_OnLevelFail;
        public static UnityEvent<LevelData> OnLevelSuccess => Instance.m_OnLevelSuccess;


        private void Awake()
        {
            m_OnLevelBegin = new UnityEvent<LevelData>();
            m_OnLevelFail = new UnityEvent<LevelData>();
            m_OnLevelSuccess = new UnityEvent<LevelData>();
        }
    }
}