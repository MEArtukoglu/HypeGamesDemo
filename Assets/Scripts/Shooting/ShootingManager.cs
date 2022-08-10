using HypeGames.Scripts.DesignPatterns.Singleton;
using HypeGames.Scripts.Shooting.Data;
using HypeGames.Scripts.Shooting.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.ObjectPoolings.SimplePooling;
using HypeGames.Scripts.ObjectPoolings.SimplePooling.Data;
using System.Runtime.CompilerServices;
using System.Linq;

namespace HypeGames.Scripts.Shooting
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    public class ShootingManager : SimpleObjectPooling
    {
        public bool IsInitialized { get; private set; }
        public static ShootingManager Instance { get; private set; }

        [SerializeField] private List<SimplePoolStack<GameObject>> m_Pools = new List<SimplePoolStack<GameObject>>();
        private List<Tuple<BulletData, SimplePoolItem<GameObject>>> m_CurrentBullets;

        private IDGenerator<double> m_BulletIDGenerator;
        [SerializeField, Range(0, 0.01f)] private float m_LinecastRate = 0.001f;

        [Header("Editor Debugging")]
        [SerializeField] private bool m_DrawBulletLinecastGizmos;
        [SerializeField] private Color m_BulletLinecastGizmosColor;
        [SerializeField] private float m_BulletLinecastGizmosSize;

        protected override List<SimplePoolStack<GameObject>> Pools { get => m_Pools; set => m_Pools = value; }

        public override void OnPoolInitialized()
        {
            if(Instance)
            {
                HypeExtensions.DebugEditor(new Exception("[ShootingManager.OnPoolInitialized] Instance already exits!"));
                return;
            }
            Instance = this;

            m_BulletIDGenerator = new IDGenerator<double>();
            m_CurrentBullets = new List<Tuple<BulletData, SimplePoolItem<GameObject>>>();
            IsInitialized = true;
        }

        private void Update()
        {
            if (!IsInitialized)
                return;

            Tuple<BulletData, SimplePoolItem<GameObject>>[] m_CurBulletsArray = m_CurrentBullets.ToArray();
            for (int i = 0; i < m_CurBulletsArray.Length; i++)
            {
                try
                {
                    m_CurBulletsArray[i].Item1.OnUpdate(Time.deltaTime);
                }
                catch(Exception exception)
                {
                    HypeExtensions.DebugEditor(new Exception($"[ShootingManager.Update] Failed to Update weapon bullet! [Index:{i}] [BulletID:{m_CurBulletsArray[i].Item1.BulletID}] See next log for exception:"));
                    HypeExtensions.DebugEditor(exception);
                    continue;
                }
            }
        }

        public bool SpawnBullet(BulletSpawnArgs bulletSpawnArgs, out BulletData spawnedBullet, UnityAction<BulletData, RaycastHit> OnBulletHit = null, UnityAction<BulletData> OnBulletMaxRangeReached = null, [CallerMemberName] string callerMemberName = "")
        {
            if(SpawnPoolItem(bulletSpawnArgs.TrailTag, out SimplePoolItem<GameObject> spawnedTrail))
            {
                double newBulletID = m_BulletIDGenerator.GenerateID();
                BulletData newBullet = new BulletData(newBulletID, spawnedTrail.Object.GetComponent<TrailRenderer>(), bulletSpawnArgs.BulletSpeed, m_LinecastRate, bulletSpawnArgs.MaxRange, bulletSpawnArgs.FirePoint.position, bulletSpawnArgs.FirePoint.forward, OnBulletHit, OnBulletMaxRangeReached);
                m_CurrentBullets.Add(new Tuple<BulletData, SimplePoolItem<GameObject>>(newBullet, spawnedTrail));
                spawnedBullet = newBullet;
                return true;
            }
            else
            {
                HypeExtensions.DebugEditor(new Exception($"[ShootingManager.SpawnBullet] Failed to find trail tag in pool! [TrailTag:{bulletSpawnArgs.TrailTag}] [CallerMemberName:{callerMemberName}]"));
                spawnedBullet = null;
                return false;
            }
        }

        public void DestroyBullet(double BulletID, [CallerMemberName] string callerMemberName = "")
        {
            Tuple<BulletData, SimplePoolItem<GameObject>> bulletData = m_CurrentBullets.FirstOrDefault(X => X.Item1.BulletID == BulletID);

            if(bulletData == null)
            {
                HypeExtensions.DebugEditor(new Exception($"[ShootingManager.DestroyBullet] Failed to find bullet! [BulletID:{BulletID}] [CallerMemberName:{callerMemberName}]"));
            }
            else
            {
                bulletData.Item1.Destroy();
                m_CurrentBullets.Remove(bulletData);
                DeSpawnPoolItem(bulletData.Item2);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(m_DrawBulletLinecastGizmos && Application.isPlaying)
            {
                Tuple<BulletData, SimplePoolItem<GameObject>>[] m_CurBullets = m_CurrentBullets.ToArray();
                foreach (Tuple<BulletData, SimplePoolItem<GameObject>> bulletData in m_CurBullets)
                {
                    bulletData.Item1.OnDrawGizmos(m_BulletLinecastGizmosColor, m_BulletLinecastGizmosSize);
                }
            }
        }
#endif
    }
}