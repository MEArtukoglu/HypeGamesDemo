using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.UI.ScriptableObjects
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>
    
    [CreateAssetMenu(fileName = "UIManagerSOData", menuName = "HypeGames/SOData/UIManagerSO")]
    public class UIManagerSO : ScriptableObject
    {
        [SerializeField] private Canvas m_CanvasPrefab;

        public Canvas CanvasPrefab => m_CanvasPrefab;
    }
}