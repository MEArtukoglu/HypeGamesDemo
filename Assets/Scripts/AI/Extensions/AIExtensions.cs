using HypeGames.Scripts.AI.Data;
using HypeGames.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.AI.Extensions
{
    public static class AIExtensions
    {
        public static void ResetAll(this AIController[] aiControllers, bool SetActive = false)
        {
            foreach (AIController ai in aiControllers)
            {
                ai.SetEnabled(AITarget.NULL, AISpawnArgs.NULL);
                ai.gameObject.SetActive(SetActive);
            }
        }
    }
}