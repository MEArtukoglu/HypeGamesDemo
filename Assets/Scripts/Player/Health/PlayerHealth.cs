using HypeGames.Scripts.Animators;
using HypeGames.Scripts.Events;
using HypeGames.Scripts.Game.Data;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.Player.Health.Data;
using HypeGames.Scripts.Player.Health.Hitbox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HypeGames.Scripts.Player.Health
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private ObserverVariable<int> m_Health;
        [SerializeField] private CharacterAnimator m_CharacterAnimator;
        private List<PlayerHealthDamageData> m_ReceivedDamages;
        [SerializeField] private Transform m_HealthBar;
        [SerializeField] private Transform m_HealthBarPivot;
        [SerializeField] private UnityEvent<PlayerHealth, PlayerHealthDamageData[]> m_OnPlayerDead;


        public ObserverVariable<int> Health => m_Health;
        public UnityEvent<PlayerHealth, PlayerHealthDamageData[]> OnPlayerDead => m_OnPlayerDead;
        public bool IsDead => m_Health.Value <= 0;

        private void Awake()
        {
            m_Health = new ObserverVariable<int>(100, true);
            m_Health.OnValueChanged += OnHealthChanged;
            m_ReceivedDamages = new List<PlayerHealthDamageData>();
            GameEvents.OnLevelBegin.AddListener(OnLevelBegin);
        }

        private void LateUpdate()
        {
            m_HealthBar.transform.LookAt(Camera.main.transform.position);
        }

        public void ApplyDamage(PlayerHitbox senderHitbox, PlayerHealthDamageData data)
        {
            if (!IsDead)
            {
                m_Health.Value -= (int)data.DamageAmount;
                m_ReceivedDamages.Add(data);
            }
        }

        public void OnHealthChanged(int previousValue, int newValue)
        {
            m_HealthBarPivot.transform.localScale = new Vector3(Mathf.Clamp((float)newValue / 100, 0, 1), m_HealthBarPivot.transform.localScale.y, m_HealthBarPivot.transform.localScale.z);
            if(newValue <= 0)
            {
                m_HealthBar.gameObject.SetActive(false);
                m_OnPlayerDead?.Invoke(this, m_ReceivedDamages.ToArray());
            }
            else
            {
                m_HealthBar.gameObject.SetActive(true);
            }
            m_CharacterAnimator.OnPlayerDeadChanged(newValue <= 0);
        }

        public void OnLevelBegin(LevelData levelData)
        {
            m_Health.Value = 100;
            m_CharacterAnimator.OnReset();
        }
    }
}