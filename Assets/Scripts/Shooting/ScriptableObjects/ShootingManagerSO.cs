using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Shooting.ScriptableObjects
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    [CreateAssetMenu(fileName = "ShootingManagerSODefault", menuName = "HypeGames/SOData/ShootingManagerSODefault")]
    public class ShootingManagerSO : ScriptableObject
    {
        [SerializeField] private int m_MaxBulletCount;
        [SerializeField] private TrailRenderer m_BulletTrailPrefab;

        public int MaxBulletCount => m_MaxBulletCount;
        public TrailRenderer BulletTrailPrefab => m_BulletTrailPrefab;
    }
}