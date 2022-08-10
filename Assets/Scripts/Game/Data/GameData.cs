using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Game.Data
{
    /// <summary>
    /// Written by Maruf Emir ARTUKO�LU
    /// MIT License
    /// </summary>

    [System.Serializable]
    public struct GameData
    {
        [SerializeField] private int m_LevelID;

        public int LevelID => m_LevelID;
    }
}