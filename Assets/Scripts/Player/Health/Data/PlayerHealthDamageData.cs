using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Player.Health.Data
{
    [System.Serializable]
    public struct PlayerHealthDamageData
    {
        public Transform DamagerAI { get; private set; }
        public float? DamageAmount { get; set; }

        public PlayerHealthDamageData(Transform DamagerAI, float DamageAmount)
        {
            this.DamagerAI = DamagerAI;
            this.DamageAmount = DamageAmount;
        }

        public PlayerHealthDamageData NULL
        {
            get
            {
                PlayerHealthDamageData nullReceivedDamageData = new PlayerHealthDamageData();
                nullReceivedDamageData.DamagerAI = null;
                nullReceivedDamageData.DamageAmount = null;
                return nullReceivedDamageData;
            }
        }
    }
}