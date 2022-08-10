using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace HypeGames.Scripts.Utilities
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>
    public class SpawnPoints : MonoBehaviour
    {
        [SerializeField] private Color m_GizmosColor = new Color(255,0,0,0.5f);
        [SerializeField] private float m_GizmosRadius = .2f;

        [SerializeField] private int m_ID;
        [SerializeField] private Transform[] m_Points;
        private int m_LastUsedPoint = -1;

        public int ID => m_ID;
        public Transform[] Points { get { return m_Points; } set { m_Points = value; } }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (Transform point in m_Points)
            {
                if (point)
                {
                    Handles.Label(point.position, $"[ID:{m_ID}]");
                    Gizmos.color = m_GizmosColor;
                    Gizmos.DrawSphere(point.position, m_GizmosRadius);
                }
            }
        }
#endif

        public Transform GetNextPoint()
        {
            if (m_Points.Length == 0)
                return null;

            if (m_LastUsedPoint >= m_Points.Length-1)
                m_LastUsedPoint = 0;
            else
                m_LastUsedPoint++;

            return m_Points[m_LastUsedPoint];
        }

        public Transform GetRandomPoint()
        {
            return m_Points[Random.Range(0, m_Points.Length)];
        }

        public static bool TryFindSpawnPoints(int ID, out SpawnPoints spawnPoints)
        {
            List<SpawnPoints> allSpawnPoints = GameObject.FindObjectsOfType<SpawnPoints>().ToList();
            int index = allSpawnPoints.FindIndex(X => X.ID == ID);
            if(index == -1)
            {
                spawnPoints = null;
                return false;
            }
            else
            {
                spawnPoints = allSpawnPoints[index];
                return true;
            }
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SpawnPoints))]
    public class SpawnPointsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SpawnPoints spawnPointsTarget = (SpawnPoints)target;
            if(GUILayout.Button("Assaign all childs as Points"))
            {
                Undo.RecordObject(this, "Assaign all childs as Points");

                int childCount = spawnPointsTarget.transform.childCount;
                Transform[] allChildPoints = new Transform[childCount];

                for (int i = 0; i < childCount; i++)
                {
                    allChildPoints[i] = spawnPointsTarget.transform.GetChild(i);
                }

                spawnPointsTarget.Points = allChildPoints;
            }
        }
    }
#endif
}