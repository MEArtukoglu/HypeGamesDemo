using HypeGames.Scripts.AI.Health.Data;
using HypeGames.Scripts.AI.Health.Hitbox;
using HypeGames.Scripts.Animators;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.Interfaces;
using HypeGames.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HypeGames.Scripts.AI.Health
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class AIHealth : MonoBehaviour
    {
        [SerializeField] private ObserverVariable<int> m_Health;
        [SerializeField] private Transform m_HealthBar;
        [SerializeField] private Transform m_HealthBarPivot;

        private CharacterAnimator m_CharacterAnimator;
        private List<AIHealthDamageData> m_ReceivedDamages;
        public AIHealthDamageData[] ReceivedDamages => m_ReceivedDamages.ToArray();

        [SerializeField] private UnityEvent<AIHealth, AIHealthDamageData[]> m_OnDead;

        public ObserverVariable<int> Health => m_Health;
        public bool IsDead => m_Health.Value <= 0;
        public UnityEvent<AIHealth, AIHealthDamageData[]> OnDead => m_OnDead;

        private void Awake()
        {
            m_Health = new ObserverVariable<int>(100, true);
            m_Health.OnValueChanged += OnHealthChanged;
            m_CharacterAnimator = GetComponent<CharacterAnimator>();
            m_ReceivedDamages = new List<AIHealthDamageData>();
            m_OnDead = new UnityEvent<AIHealth, AIHealthDamageData[]>();
        }

        private void LateUpdate()
        {
            m_HealthBar.transform.LookAt(Camera.main.transform.position);
        }

        public void ApplyDamage(AIHitboxPart senderHitbox, AIHealthDamageData data)
        {
            if (!IsDead)
            {
                m_Health.Value -= (int)data.DamageAmount;
                m_ReceivedDamages.Add(data);
            }
        }

        private void OnEnable()
        {
            m_Health.Value = 100;
            m_ReceivedDamages.Clear();
        }

        public void OnHealthChanged(int previousValue, int newValue)
        {
            m_HealthBar.gameObject.SetActive(newValue > 0);
            m_HealthBarPivot.transform.localScale = new Vector3(Mathf.Clamp((float)newValue/100, 0, 1), m_HealthBarPivot.transform.localScale.y, m_HealthBarPivot.transform.localScale.z);
            if (newValue <= 0)
            {
                m_HealthBar.gameObject.SetActive(false);
                m_CharacterAnimator.OnPlayerDeadChanged(newValue <= 0);
                OnDead?.Invoke(this, m_ReceivedDamages.ToArray());
            }
            else
            {
                m_HealthBar.gameObject.SetActive(true);
            }
        }
    }
}