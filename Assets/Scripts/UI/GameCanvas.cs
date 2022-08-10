using HypeGames.Scripts.Events;
using HypeGames.Scripts.Game;
using HypeGames.Scripts.Game.Data;
using HypeGames.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HypeGames.Scripts.UI
{
    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> m_Panels = new List<RectTransform>();
        [SerializeField] private UICrosshair m_UICrosshair;
        [SerializeField] private Button m_TapToStartBtn;
        [SerializeField] private Button m_NextLevelBtn;
        [SerializeField] private Button m_RestartBtn;
        [SerializeField] private Button m_FailRestartBtn;
        [SerializeField] private TextMeshProUGUI m_ScoreTxt;
        [SerializeField] private TextMeshProUGUI m_LevelTxt;


        public UICrosshair UICrosshair => m_UICrosshair;

        private void Awake()
        {
            GameEvents.OnLevelBegin.AddListener(OnLevelBegin);
            GameEvents.OnLevelFail.AddListener(X => SwitchPanel(2));
            GameEvents.OnLevelSuccess.AddListener(X => SwitchPanel(3));
            m_TapToStartBtn.onClick.AddListener(() => GameManager.Instance.LoadCurrentLevel());
            m_NextLevelBtn.onClick.AddListener(() => GameManager.Instance.LoadCurrentLevel());
            m_RestartBtn.onClick.AddListener(() => GameManager.Instance.LoadCurrentLevel());
            m_FailRestartBtn.onClick.AddListener(() => GameManager.Instance.LoadCurrentLevel());
        }

        public void SwitchPanel(int Index)
        {
            for (int i = 0; i < m_Panels.Count; i++)
            {
                if (m_Panels[i].gameObject)
                    m_Panels[i].gameObject.SetActive(i == Index);
            }
        }

        private void Update()
        {
            LevelData activeLevel = LevelManager.Instance.ActiveLevel;
            if(activeLevel)
            {
                int leftEnemyCount = activeLevel.MaxAICount - AISpawnManager.Instance.DeadAICount;
                m_ScoreTxt.text = $"Enemies: {leftEnemyCount}/{activeLevel.MaxAICount}";
            }
        }

        public void OnLevelBegin(LevelData levelData)
        {
            m_UICrosshair.OnReset();
            SwitchPanel(1);
        }

        public void SetCurrentLevelText(int CurLevel)
        {
            m_LevelTxt.text = $"Level: {CurLevel}";
        }
    }
}