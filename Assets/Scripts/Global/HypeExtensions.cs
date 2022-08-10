using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace HypeGames.Scripts.Global
{
    public static class HypeExtensions
    {
        /// <summary>
        /// Written by Maruf Emir ARTUKOÐLU
        /// MIT License
        /// </summary>
        
        public static void DebugEditor(Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogException(exception);
#endif
        }

        public static void DebugEditor(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        public static bool CalculateRandomDestination(this NavMeshAgent navMeshAgent, float Range, out Vector3 Result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = navMeshAgent.transform.position + Random.insideUnitSphere * Range;
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
                {
                    Result = navHit.position;
                    return true;
                }
            }

            Result = default;
            return false;
        }

        public static List<Transform> GetAllChilds(this Transform _transform, List<Transform> childs)
        {
            foreach (Transform t in _transform)
            {
                childs.Add(t);
                GetAllChilds(t, childs);
            }

            return childs;
        }
    }
}