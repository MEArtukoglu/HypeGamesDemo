using HypeGames.Scripts.AI.Health.Data;
using HypeGames.Scripts.AI.Health.Hitbox;
using HypeGames.Scripts.Character;
using HypeGames.Scripts.Global;
using HypeGames.Scripts.Interfaces;
using HypeGames.Scripts.Shooting;
using HypeGames.Scripts.Shooting.Data;
using HypeGames.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Player
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    public class PlayerController : MonoBehaviour
    {
        private UIManager UIManager;
        private UICrosshair UICrosshair;

        [SerializeField] private CubeCharacterRig m_CubeCharacterRig;
        private RaycastHit? lastKnownHitInfo;

        [SerializeField] private Transform m_ShotTransform;
        [SerializeField] private float m_FireRate;
        [SerializeField] private BulletSpawnArgs m_BulletSpawnArgs;

        private float lastFireTime;

        private void Awake()
        {
            UIManager = UIManager.Instance;
            UICrosshair = this.UIManager.UICrosshair;
            lastKnownHitInfo = null;
            lastFireTime = default;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(UICrosshair.Crosshair.rectTransform.position);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                lastKnownHitInfo = hitInfo;
                if(hitInfo.transform.gameObject.TryGetComponent(out IDamageable<AIHealthDamageData> iDamageable))
                {
                    if (!iDamageable.GetIsDead())
                    {
                        float nextFireTime = lastFireTime + m_FireRate;
                        if (Time.time >= nextFireTime)
                        {
                            ShootingManager.Instance.SpawnBullet(m_BulletSpawnArgs, out BulletData bulletData, this.OnBulletHit, this.OnBulletMaxRangeReached);
                            UICrosshair.SetTargetStatus(true);
                            lastFireTime = Time.time;
                        }
                    }
                    else
                    {
                        UICrosshair.SetTargetStatus(false);
                    }
                }
                else
                {
                    UICrosshair.SetTargetStatus(false);
                }
            }
            else
            {
                UICrosshair.SetTargetStatus(false);
            }
        }

        private void LateUpdate()
        {
            if(lastKnownHitInfo != null)
            {
                m_CubeCharacterRig.UpdateCharacterRotation(Time.deltaTime, lastKnownHitInfo.Value.point);
                m_CubeCharacterRig.UpdateHandRotation(Time.deltaTime, lastKnownHitInfo.Value.point);
            }
        }

        public void OnBulletHit(BulletData bulletData, RaycastHit raycastHit)
        {
            if(raycastHit.transform.TryGetComponent(out IDamageable<AIHealthDamageData> iDamageable))
            {
                float damageAmount = default;
                HitboxID hitboxID = iDamageable.GetHitboxID();
                switch(hitboxID)
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
                iDamageable.ApplyDamage(new AIHealthDamageData(transform, damageAmount));
            }

            ShootingManager.Instance.DestroyBullet(bulletData.BulletID);
        }

        public void OnBulletMaxRangeReached(BulletData bulletData)
        {
            ShootingManager.Instance.DestroyBullet(bulletData.BulletID);
        }
    }
}