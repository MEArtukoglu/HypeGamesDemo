using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.DesignPatterns.Singleton
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    public class SingletonPattern<T> : MonoBehaviour where T : SingletonPattern<T>
    {
        private static T m_Instance;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject newInstance = new GameObject($"[Singleton] {typeof(T).Name}");
                    m_Instance = newInstance.AddComponent<T>();
                }
                return m_Instance;
            }
        }
    }
}