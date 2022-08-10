using HypeGames.Scripts.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HypeGames.Scripts.Shooting.Data
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    [System.Serializable]
    public class BulletData
    {
        private double m_BulletID;
        private TrailRenderer m_TrailRenderer;

        private Vector3 m_SpawnPos;
        private float m_BulletRange;


        private float m_BulletSpeed;
        private float m_LinecastRate;
        
        private float m_LastLinecastTime;
        private Vector3 m_LastLinecastPos;

        private UnityAction<BulletData, RaycastHit> OnBulletHit;
        private UnityAction<BulletData> OnBulletMaxRangeReached;

        private List<Vector3> m_LinecastPoses { get; set; } = default;

        public double BulletID => m_BulletID;
        public Transform m_Transform => m_TrailRenderer.transform;


        public BulletData(double BulletID, TrailRenderer TrailRenderer, float BulletSpeed, float LinecastRate, float BulletRange, Vector3 SpawnPoint, Vector3 Direction, UnityAction<BulletData, RaycastHit> OnBulletHit = null, UnityAction<BulletData> OnBulletMaxRangeReached = null)
        {
            this.m_BulletID = BulletID;
            this.m_TrailRenderer = TrailRenderer;
            this.m_BulletSpeed = BulletSpeed;
            this.m_LinecastRate = LinecastRate;

            this.m_LastLinecastTime = Time.time;
            this.m_BulletRange = BulletRange;
            
            this.m_LastLinecastPos = default;
            this.m_LinecastPoses = new List<Vector3>();

            this.OnBulletHit = OnBulletHit;
            this.OnBulletMaxRangeReached = OnBulletMaxRangeReached;

            this.Spawn(SpawnPoint, Direction);
        }


        public void OnUpdate(float deltaTime)
        {
            float distance = Vector3.Distance(m_SpawnPos, m_Transform.position);
            if(distance >= m_BulletRange)
            {
                OnBulletMaxRangeReached?.Invoke(this);
            }

            m_Transform.Translate(Vector3.forward * m_BulletSpeed * deltaTime);
            float nextLinecastTime = m_LastLinecastTime + m_LinecastRate;

            if(Time.time >= nextLinecastTime)
            {
                Vector3 curPos = m_Transform.position;
                if(Physics.Linecast(m_LastLinecastPos, curPos, out RaycastHit hitInfo))
                {
                    OnBulletHit?.Invoke(this, hitInfo);
                }
                m_LinecastPoses.Add(curPos);
                m_LastLinecastPos = m_Transform.position;
                m_LastLinecastTime = Time.time;
            }
        }


        public void Spawn(Vector3 SpawnPoint, Vector3 Direction)
        {
            m_Transform.position = SpawnPoint;
            m_Transform.forward = Direction;
            m_SpawnPos = m_Transform.position; 
            m_TrailRenderer.Clear();
            m_TrailRenderer.gameObject.SetActive(true);
        }

        public void Destroy()
        {
            m_TrailRenderer.Clear();
            m_TrailRenderer.gameObject.SetActive(false);
        }

        public void OnDrawGizmos(Color gizmosColor, float gizmosSize)
        {
#if UNITY_EDITOR
            Gizmos.color = gizmosColor;
            Vector3[] linecastPoses = m_LinecastPoses.ToArray();
            foreach (Vector3 linecastPos in linecastPoses)
            {
                Gizmos.DrawCube(linecastPos, Vector3.one * gizmosSize);
            }
#endif
        }
    }
}