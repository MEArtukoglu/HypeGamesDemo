using HypeGames.Scripts.AI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace HypeGames.Scripts.AI
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class AIBehaviour : MonoBehaviour
    {
        /// <summary>
        /// !!---Make sure using Try-Catch and Manual Exception handling when calling OnUpdate();---!!
        /// </summary>
        public abstract void OnUpdate(UnityAction<Exception> OnException = null);

        /// <summary>
        /// OnInitialized must be called from typeof Awake function!
        /// !!---Make sure using Try-Catch and Manual Exception handling when calling OnInitialized();---!!
        /// </summary>
        public abstract void OnInitialized(UnityAction<Exception> OnException = null);

        public abstract void SetEnabled(AITarget newTarget, AISpawnArgs spawnArgs);
    }
}