using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.AI.Health.Data
{
    [System.Serializable]
    public struct AIHealthDamageData
    {
        public Transform DamagerPlayer { get; private set; }
        public float? DamageAmount { get; set; }

        public AIHealthDamageData(Transform DamagerPlayer, float DamageAmount)
        {
            this.DamagerPlayer = DamagerPlayer;
            this.DamageAmount = DamageAmount;
        }

        public AIHealthDamageData NULL
        {
            get
            {
                AIHealthDamageData nullReceivedDamageData = new AIHealthDamageData();
                nullReceivedDamageData.DamagerPlayer = null;
                nullReceivedDamageData.DamageAmount = null;
                return nullReceivedDamageData;
            }
        }
    }
}