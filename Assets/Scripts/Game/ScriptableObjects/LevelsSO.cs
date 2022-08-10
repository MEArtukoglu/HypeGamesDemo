using HypeGames.Scripts.Game.Data;
using HypeGames.Scripts.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Game.ScriptableObjects
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    [CreateAssetMenu(fileName = "LevelsSODefault", menuName = "HypeGames/SOData/LevelsSO")]
    public class LevelsSO : ScriptableObject
    {
        [SerializeField] private List<LevelData> m_Levels = new List<LevelData>();
        public List<LevelData> Levels => m_Levels;

        public bool TryFindLevel(int LevelID, out LevelData levelData)
        {
            int index = m_Levels.FindIndex(X => X.LevelID == LevelID);
            if (index.Equals(-1))
            {
                levelData = LevelData.NULL;
                return false;
            }
            else
            {
                levelData = m_Levels[index];
                return true;
            }
        }
    }
}