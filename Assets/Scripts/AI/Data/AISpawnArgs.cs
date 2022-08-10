using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.AI.Data
{
    [System.Serializable]
    public struct AISpawnArgs
    {
        public float AgentDestinationRange;
        public float AgentFinishDistance;
        public float AgentNewDesRandomMinTime;
        public float AgentNewDesRandomMaxTime;
        public float AgentFireRate;
        public float AgentSpeed;

        public AISpawnArgs(float AgentDestinationRange, float AgentFinishDistance, float AgentNewDesRandomMinTime, float AgentNewDesRandomMaxTime, float AgentFireRate, float AgentSpeed)
        {
            this.AgentDestinationRange = AgentDestinationRange;
            this.AgentFinishDistance = AgentFinishDistance;
            this.AgentNewDesRandomMinTime = AgentNewDesRandomMinTime;
            this.AgentNewDesRandomMaxTime = AgentNewDesRandomMaxTime;
            this.AgentFireRate = AgentFireRate;
            this.AgentSpeed = AgentSpeed;
        }

        public static AISpawnArgs NULL => new AISpawnArgs(-1, -1, -1, -1, -1, -1);

        public static implicit operator bool(AISpawnArgs aiArgs)
        {
            return aiArgs.AgentDestinationRange != -1 && aiArgs.AgentFinishDistance != -1 && aiArgs.AgentNewDesRandomMinTime != -1 && aiArgs.AgentNewDesRandomMaxTime != 1 && aiArgs.AgentFireRate != -1 && aiArgs.AgentSpeed != -1;
        }
    }
}