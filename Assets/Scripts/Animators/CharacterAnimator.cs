using HypeGames.Scripts.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Animators
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        List<Tuple<Transform, Vector3, Quaternion>> m_RigChilds;

        protected virtual void Awake()
        {
            m_RigChilds = new List<Tuple<Transform, Vector3, Quaternion>>();
            List<Transform> allChilds = new List<Transform>();
            transform.GetAllChilds(allChilds);
            for (int i = 0; i < allChilds.Count; i++)
            {
                m_RigChilds.Add(new Tuple<Transform, Vector3, Quaternion>(allChilds[i], allChilds[i].localPosition, allChilds[i].localRotation));
            }
        }


        public void OnPlayerDeadChanged(bool value)
        {
            m_Animator.SetBool("IsDead", value);
        }

        public void OnReset()
        {
            for (int i = 0; i < m_RigChilds.Count; i++)
            {
                Tuple<Transform, Vector3, Quaternion> iRigChild = m_RigChilds[i];
                iRigChild.Item1.localPosition = iRigChild.Item2;
                iRigChild.Item1.localRotation = iRigChild.Item3;
            }
        }
    }
}