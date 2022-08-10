using HypeGames.Scripts.AI.Health.Hitbox;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.Interfaces;
using HypeGames.Scripts.Player.Health.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Player.Health.Hitbox
{
    public class PlayerHitbox : MonoBehaviour, IDamageable<PlayerHealthDamageData>
    {
        [SerializeField] private PlayerHealth m_PlayerHealth = default;
        [SerializeField] private HitboxID m_HitboxID = default;
        public PlayerHealth PlayerHealth => m_PlayerHealth;
        public HitboxID HitboxID => m_HitboxID;

        public void ApplyDamage(PlayerHealthDamageData damageData)
        {
            m_PlayerHealth.ApplyDamage(this, damageData);
        }

        public HitboxID GetHitboxID()
        {
            return m_HitboxID;
        }

        public bool GetIsDead()
        {
            return m_PlayerHealth.IsDead;
        }
    }
}