using HypeGames.Scripts.Global;
using HypeGames.Scripts.ObjectPoolings.SimplePooling.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HypeGames.Scripts.ObjectPoolings.SimplePooling
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    public abstract class SimpleObjectPooling : MonoBehaviour
    {
        protected abstract List<SimplePoolStack<GameObject>> Pools { get; set; }
        private Dictionary<string, List<SimplePoolItem<GameObject>>> m_CurrentPool;


        public abstract void OnPoolInitialized();

        private void Awake()
        {
            m_CurrentPool = new Dictionary<string, List<SimplePoolItem<GameObject>>>();
            for (int i = 0; i < Pools.Count; i++)
            {
                SimplePoolStack<GameObject> iPoolStack = Pools[i];
                for (int j = 0; j < iPoolStack.Count; j++)
                {
                    GameObject newPoolObject = Instantiate(iPoolStack.Prefab, transform, true);
                    newPoolObject.gameObject.SetActive(false);
                    SimplePoolItem<GameObject> newPoolItem = new SimplePoolItem<GameObject>(iPoolStack.Tag, newPoolObject, false);
                    
                    if (m_CurrentPool.TryGetValue(iPoolStack.Tag, out List<SimplePoolItem<GameObject>> value))
                    {
                        value.Add(newPoolItem);
                        m_CurrentPool[iPoolStack.Tag] = value;
                    }
                    else
                    {
                        List<SimplePoolItem<GameObject>> newValue = new List<SimplePoolItem<GameObject>>();
                        newValue.Add(newPoolItem);
                        m_CurrentPool.Add(iPoolStack.Tag, newValue);
                    }
                }
            }

            OnPoolInitialized();
        }

        protected bool SpawnPoolItem(string Tag, out SimplePoolItem<GameObject> spawnedObject, [CallerMemberName] string callerMemberName = "")
        {
            if(m_CurrentPool.TryGetValue(Tag, out List<SimplePoolItem<GameObject>> value))
            {
                int index = value.FindIndex(X => X.IsInUse == false);
                if(index == -1)
                {
                    HypeExtensions.DebugEditor(new Exception($"[DefaultObjectPooling.SpawnPoolItem] Failed to find not in use object! [Tag:{Tag}] [CallerMemberName:{callerMemberName}]"));
                    spawnedObject = default;
                    return false;
                }
                else
                {
                    value[index] = new SimplePoolItem<GameObject>(value[index].Tag, value[index].Object, true);
                    m_CurrentPool[Tag] = value;
                    spawnedObject = value[index];
                    return true;
                }
            }
            else
            {
                HypeExtensions.DebugEditor(new Exception("[DefaultObjectPooling.SpawnPoolItem] Failed to find given tag! [CallerMemberName:{callerMemberName}]"));
                spawnedObject = default;
                return false;
            }
        }

        protected bool DeSpawnPoolItem(SimplePoolItem<GameObject> Object, [CallerMemberName] string callerMemberName = "")
        {
            if (m_CurrentPool.TryGetValue(Object.Tag, out List<SimplePoolItem<GameObject>> value))
            {
                int index = value.FindIndex(X => X.Object == Object.Object && X.Tag == Object.Tag);
                if(index != -1)
                {
                    value[index] = new SimplePoolItem<GameObject>(Object.Tag, Object.Object, false);
                    return true;
                }
                else
                {
                    HypeExtensions.DebugEditor(new Exception($"[DefaultObjectPooling.DeSpawnPoolItem] Failed to find index! [CallerMemberName:{callerMemberName}]"));
                    return false;
                }
            }
            else
            {
                HypeExtensions.DebugEditor(new Exception($"[DefaultObjectPooling.DeSpawnPoolItem] Failed to find given tag! [CallerMemberName:{callerMemberName}]"));
                return false;
            }
        }
    }
}