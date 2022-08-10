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
    public struct SimplePoolItem<T>
    {
        [SerializeField] private string m_Tag;
        [SerializeField] private T m_Object;
        [SerializeField] private bool m_IsInUse;

        public string Tag => m_Tag;
        public T Object => m_Object;
        public bool IsInUse => m_IsInUse;

        public SimplePoolItem(string Tag, T Object, bool IsInUse = false)
        {
            this.m_Tag = Tag;
            this.m_Object = Object;
            this.m_IsInUse = IsInUse;
        }
    }
}