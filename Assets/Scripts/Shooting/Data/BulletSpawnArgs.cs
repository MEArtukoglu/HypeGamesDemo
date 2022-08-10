using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HypeGames.Scripts.Shooting.Data
{
    [System.Serializable]
    public struct BulletSpawnArgs
    {
        public Transform FirePoint;
        public string TrailTag;
        public float BulletSpeed;
        public float MaxRange;
    }
}