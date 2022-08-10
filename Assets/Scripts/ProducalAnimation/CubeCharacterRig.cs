using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Character
{
    [System.Serializable]
    public class CubeCharacterRig
    {
        [SerializeField] private Transform m_Character;
        [SerializeField] private float m_CharacterInterpole;

        [SerializeField] private Transform m_AimHand;
        [SerializeField] private float m_AimHandInterpole;

        [SerializeField] private Vector3 m_CharacterAxis;
        [SerializeField] private Vector3 m_AimHandAxis;

        public Transform Character => m_Character;
        public Transform AimHand => m_AimHand;

        public void UpdateHandRotation(float deltaTime, Vector3 PointToLook)
        {
            Vector3 rotation = Quaternion.LookRotation(PointToLook - m_AimHand.position, Vector3.up).eulerAngles;
            Vector3 lookEuler = new Vector3(m_AimHandAxis.x * rotation.x, m_AimHandAxis.y * rotation.y, m_AimHandAxis.z * rotation.z);
            m_AimHand.rotation = Quaternion.Lerp(m_AimHand.rotation, Quaternion.Euler(lookEuler), m_AimHandInterpole * deltaTime);
        }

        public void UpdateCharacterRotation(float deltaTime, Vector3 PointToLook)
        {
            Vector3 rotation = Quaternion.LookRotation(PointToLook - m_Character.position, Vector3.up).eulerAngles;
            Vector3 lookEuler = new Vector3(m_CharacterAxis.x * rotation.x, m_CharacterAxis.y * rotation.y, m_CharacterAxis.z * rotation.z);
            m_Character.rotation = Quaternion.Lerp(m_Character.rotation, Quaternion.Euler(lookEuler), m_CharacterInterpole * deltaTime);
        }
    }
}