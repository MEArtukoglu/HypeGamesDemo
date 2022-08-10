using HypeGames.Scripts.AI.Health.Data;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.AI.Health.Hitbox
{
    public class AIHitboxPart : MonoBehaviour, IDamageable<AIHealthDamageData>
    {
        [SerializeField] private AIHealth m_AIHealth = default;
        [SerializeField] private HitboxID m_HitboxID = default;
        public AIHealth AIHealth => m_AIHealth;
        public HitboxID HitboxID => m_HitboxID;
       
        public void ApplyDamage(AIHealthDamageData damageData)
        {
            m_AIHealth.ApplyDamage(this, damageData);
        }

        public HitboxID GetHitboxID()
        {
            return m_HitboxID;
        }

        public bool GetIsDead()
        {
            return m_AIHealth.IsDead;
        }
    }
}