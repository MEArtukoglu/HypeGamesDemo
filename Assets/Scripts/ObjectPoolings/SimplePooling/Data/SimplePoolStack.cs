using HypeGames.Scripts.ObjectPoolings.SimplePooling.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.ObjectPoolings.SimplePooling.Data
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    [System.Serializable]
    public struct SimplePoolStack<T> where T : class
    {
        [SerializeField] private string m_Tag;
        [SerializeField] private T m_Prefab;
        [SerializeField] private int m_Count;

        public string Tag => m_Tag;
        public T Prefab => m_Prefab;
        public int Count => m_Count;
    }
}