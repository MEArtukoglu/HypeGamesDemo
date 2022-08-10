using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.AI.ScriptableObjects
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    [CreateAssetMenu(fileName = "AISpawnManagerSO", menuName = "HypeGames/SOData/AISpawnManagerSODefault")]
    public class AISpawnManagerSO : ScriptableObject
    {
        [SerializeField] private AIController m_EnemyAIPrefab;
        public AIController EnemyAIPrefab => m_EnemyAIPrefab;
    }
}