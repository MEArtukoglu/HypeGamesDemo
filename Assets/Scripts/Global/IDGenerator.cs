using System;
using UnityEngine;

namespace HypeGames.Scripts.Global
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    [System.Serializable]
    public class IDGenerator<T> where T : IConvertible
    {
        [SerializeField] private T m_LastGeneratedID;

        public T GenerateID()
        {
            if (m_LastGeneratedID.GetType() == typeof(double))
            {
                double m_LastGenerateIDAsDouble = (m_LastGeneratedID as double?) ?? 0;
                m_LastGenerateIDAsDouble++;
                m_LastGeneratedID = m_LastGenerateIDAsDouble as dynamic;
                return m_LastGenerateIDAsDouble as dynamic;
            }
            else if (m_LastGeneratedID.GetType() == typeof(int))
            {
                int m_LastGenerateIDAsFloat = (m_LastGeneratedID as int?) ?? 0;
                m_LastGenerateIDAsFloat++;
                m_LastGeneratedID = m_LastGenerateIDAsFloat as dynamic;
                return m_LastGenerateIDAsFloat as dynamic;
            }
            else
            {
                HypeExtensions.DebugEditor(new Exception($"[IDGenerator] Current type is not supported! [Type:{m_LastGeneratedID.GetType()}]"));
                return m_LastGeneratedID;
            }
        }
    }
}