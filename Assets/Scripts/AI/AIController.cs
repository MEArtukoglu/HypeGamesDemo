using System;
using System.Collections;
using System.Collections.Generic;
using HypeGames.Scripts.AI.Data;
using HypeGames.Scripts.AI.Health;
using HypeGames.Scripts.AI.Health.Data;
using HypeGames.Scripts.AI.Health.Hitbox;
using HypeGames.Scripts.Character;
using HypeGames.Scripts.Events;
using HypeGames.Scripts.Game.Data;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.Interfaces;
using HypeGames.Scripts.Player;
using HypeGames.Scripts.Player.Health.Data;
using HypeGames.Scripts.Shooting;
using HypeGames.Scripts.Shooting.Data;
using HypeGames.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace HypeGames.Scripts.AI
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>
    
    [RequireComponent(typeof(AIHealth))]
    public class AIController : AIBehaviour
    {
        private AIHealth m_AIHealth;
        private NavMeshAgent m_NavMeshAgent;
        private AITarget m_CurTarget;
        private AISpawnArgs m_SpawnArgs;

        private float? m_AgentDelayEndTime;

        [SerializeField] private CubeCharacterRig m_CubeCharacterRig;
        [SerializeField] private Transform m_ShotTransform;
        [SerializeField] private BulletSpawnArgs m_BulletSpawnArgs;

        private PlayerController m_Player;
        private float lastFireTime;

        public AITarget CurTarget => m_CurTarget;

        public override void OnInitialized(UnityAction<Exception> OnException = null)
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_AIHealth = GetComponent<AIHealth>();
            m_Player = FindObjectOfType<PlayerController>();
            gameObject.SetActive(false);
            lastFireTime = default;
            m_AIHealth.OnDead.AddListener(OnDead);
            GameEvents.OnLevelFail.AddListener(OnLevelFail);
            this.SetEnabled(AITarget.NULL, AISpawnArgs.NULL);
        }

        public override void SetEnabled(AITarget newTarget, AISpawnArgs spawnArgs)
        {
            this.m_SpawnArgs = spawnArgs;
            if (newTarget)
            {
                if (m_NavMeshAgent.isStopped)
                    m_NavMeshAgent.isStopped = false;

                m_NavMeshAgent.speed = spawnArgs.AgentSpeed;
                m_NavMeshAgent.Warp((Vector3)newTarget.StartPosition);
                TrySetRandomDestination(newTarget);
            }
            this.m_CurTarget = newTarget;
        }

        public override void OnUpdate(UnityAction<Exception> OnException = null)
        {
            //Implicit Check
            if (m_CurTarget && m_SpawnArgs)
            {
                if(m_CurTarget.Destination != null)
                {
                    float curDis = Vector3.Distance(m_NavMeshAgent.transform.position, (Vector3)m_CurTarget.Destination);
                    if(curDis < m_SpawnArgs.AgentFinishDistance)
                    {
                        if(m_AgentDelayEndTime != null)
                        {
                            if (Time.time >= m_AgentDelayEndTime)
                                TrySetRandomDestination(m_CurTarget);
                        }
                        else
                        {
                            m_AgentDelayEndTime = Time.time + Random.Range(m_SpawnArgs.AgentNewDesRandomMinTime, m_SpawnArgs.AgentNewDesRandomMaxTime);
                        }
                    }
                }
                else
                {
                    TrySetRandomDestination(m_CurTarget);
                }

                float nextFireTime = lastFireTime + m_SpawnArgs.AgentFireRate;
                if (Time.time >= nextFireTime)
                {
                    ShootingManager.Instance.SpawnBullet(m_BulletSpawnArgs, out BulletData bulletData, this.OnBulletHit, this.OnBulletMaxRangeReached);
                    lastFireTime = Time.time;
                }
            }
        }

        private void LateUpdate()
        {
            if (m_CurTarget && m_SpawnArgs && !m_AIHealth.IsDead)
            {
                m_CubeCharacterRig.UpdateCharacterRotation(Time.deltaTime, m_Player.transform.position);
                m_CubeCharacterRig.UpdateHandRotation(Time.deltaTime, m_Player.transform.position);
            }
        }


        public void OnDead(AIHealth AIHealth, AIHealthDamageData[] damageDatas)
        {
            m_CurTarget = AITarget.NULL;
            m_SpawnArgs = AISpawnArgs.NULL;
            m_NavMeshAgent.isStopped = true;
        }

        public void OnBulletHit(BulletData bulletData, RaycastHit raycastHit)
        {
            if (raycastHit.transform.TryGetComponent(out IDamageable<PlayerHealthDamageData> iDamageable))
            {
                float damageAmount = default;
                HitboxID hitboxID = iDamageable.GetHitboxID();
                switch (hitboxID)
                {
                    case HitboxID.Arms:
                        damageAmount = 25;
                        break;
                    case HitboxID.Body:
                        damageAmount = 60;
                        break;
                    case HitboxID.Head:
                        damageAmount = 100;
                        break;
                    case HitboxID.Legs:
                        damageAmount = 25;
                        break;
                }
                iDamageable.ApplyDamage(new PlayerHealthDamageData(transform, damageAmount));
            }

            ShootingManager.Instance.DestroyBullet(bulletData.BulletID);
        }

        public void OnBulletMaxRangeReached(BulletData bulletData)
        {
            ShootingManager.Instance.DestroyBullet(bulletData.BulletID);
        }

        private void TrySetRandomDestination(AITarget aiTarget)
        {
            if (m_NavMeshAgent.CalculateRandomDestination(m_SpawnArgs.AgentDestinationRange, out Vector3 Result))
            {
                aiTarget.SetDestination(Result);
                m_NavMeshAgent.SetDestination((Vector3)aiTarget.Destination);
                m_AgentDelayEndTime = null;
                m_CurTarget = aiTarget;
            }
        }

        public void OnLevelFail(LevelData levelData)
        {
            if (m_CurTarget && m_SpawnArgs)
            {
                m_CurTarget = AITarget.NULL;
                m_SpawnArgs = AISpawnArgs.NULL;
                m_NavMeshAgent.isStopped = true;
            }
        }
    }
}