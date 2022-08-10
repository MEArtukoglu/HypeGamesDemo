using HypeGames.Scripts.AI.Health.Hitbox;
using HypeGames.Scripts.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Interfaces
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    public interface IDamageable<T>
    {
        public void ApplyDamage(T damageData);

        public HitboxID GetHitboxID();
        public bool GetIsDead();
    }
}