using UnityEngine;

namespace HypeGames.Scripts.Global
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>


    [System.Serializable]
    public class ObserverVariable<T> where T : struct
    {
        private bool m_IsInitialized;

        public T Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                if (m_IsInitialized)
                {
                    if (DontCallbackOnSameValueChange)
                    {
                        if (!m_Value.Equals(value))
                        {
                            OnValueChanged?.Invoke(m_Value, value);
                        }
                    }
                    else
                        OnValueChanged?.Invoke(m_Value, value);
                }
                m_Value = value;
            }
        }

        [SerializeField] private T m_Value;
        public delegate void OnValueChangedCallback(T previousValue, T newValue);
        public OnValueChangedCallback OnValueChanged;

        private bool DontCallbackOnSameValueChange = false;

        public ObserverVariable(T InitializeValue = default, bool DontCallbackOnSameValueChange = false)
        {
            this.m_Value = InitializeValue;
            this.DontCallbackOnSameValueChange = DontCallbackOnSameValueChange;
            this.m_IsInitialized = true;
        }
    }
}