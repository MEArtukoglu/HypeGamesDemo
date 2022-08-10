using HypeGames.Scripts.AI.Health.Hitbox;
using HypeGames.Scripts.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HypeGames.Scripts.Weapons.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponsDataSO", menuName = "HypeGames/SOData/WeaponsDataSO")]
    public class WeaponsDataSO : ScriptableObject
    {
        [SerializeField] private List<WeaponData> m_WeaponDatas;

        public WeaponData[] WeaponDatas => m_WeaponDatas.ToArray();
    }

    [System.Serializable]
    public struct WeaponData
    {
        public string WeaponName;
        public int WeaponID;
        public WeaponDamage[] WeaponDamages;
    }

    [System.Serializable]
    public struct WeaponDamage
    {
        public HitboxID HitboxID;
        public int Damage;
    }
}