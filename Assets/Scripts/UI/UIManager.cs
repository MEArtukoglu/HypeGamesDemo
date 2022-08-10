using HypeGames.Scripts.DesignPatterns.Singleton;
using HypeGames.Scripts.Events;
using HypeGames.Scripts.Game.Data;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.UI.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HypeGames.Scripts.UI
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    public class UIManager : SingletonPattern<UIManager>
    {
        private Canvas m_Canvas;
        private GameCanvas m_GameCanvas;
        private EventSystem m_EventSystem;

        private UIManagerSO m_Data;

        public Canvas Canvas => m_Canvas;
        public GameCanvas GameCanvas => m_GameCanvas;
        public UIManagerSO Data => m_Data;
        public UICrosshair UICrosshair => m_GameCanvas.UICrosshair;


        private void Awake()
        {
            #region Initialize
            UIManagerSO newData = Resources.Load<UIManagerSO>("UIManagerSODefault");
            if(newData.Equals(null))
            {
                HypeExtensions.DebugEditor(new Exception($"[UIManager.Awake] Failed to find UIManagerSO!\nPlease make sure it exits in resources folder!"));
                return;
            }
            m_Data = newData;
            #endregion
            m_Canvas = Instantiate(m_Data.CanvasPrefab);
            m_GameCanvas = m_Canvas.GetComponent<GameCanvas>();
            m_EventSystem = new GameObject("EventSystems", typeof(EventSystem), typeof(StandaloneInputModule)).GetComponent<EventSystem>();
        }
        
    }
}