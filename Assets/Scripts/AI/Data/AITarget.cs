using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.AI.Data
{
    [System.Serializable]
    public struct AITarget
    {
        /// <summary>
        /// Written by Maruf Emir ARTUKOÐLU
        /// MIT License
        /// </summary>

        [SerializeField] private bool m_IsEnabled;
        [SerializeField] private Transform m_Target;
        [SerializeField] private Vector3? m_StartPosition;
        private Vector3? m_Destination;

        public bool IsEnabled => m_IsEnabled;
        public Transform Target => m_Target;
        public Vector3? StartPosition => m_StartPosition;
        public Vector3? Destination => m_Destination;

        public void SetDestination(Vector3 newDestination) => m_Destination = newDestination;


        public AITarget(bool IsEnabled, Transform Target, Vector3? StartPosition)
        {
            this.m_IsEnabled = IsEnabled;
            this.m_Target = Target;
            this.m_StartPosition = StartPosition;
            this.m_Destination = null;
        }

        public static AITarget NULL => new AITarget(default, null, null);

        public static implicit operator bool(AITarget aiTarget)
        {
            return (aiTarget.IsEnabled && aiTarget.Target != null && aiTarget.StartPosition != null);
        }
    }
}