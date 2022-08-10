using HypeGames.Scripts.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HypeGames.Scripts.UI
{
    /// <summary>
    /// Written by Maruf Emir ARTUKOÐLU
    /// MIT License
    /// </summary>

    public class UICrosshair : MonoBehaviour, IDragHandler
    {
        private Canvas m_Canvas;
        [SerializeField] private Image m_Crosshair;

        [SerializeField] private Color m_DefaultColor;
        [SerializeField] private Color m_TargetColor;

        private Tuple<Vector3, Quaternion> m_InitializeData;

        public Image Crosshair => m_Crosshair;
        public Color DefaultColor => m_DefaultColor;
        public Color TargetColor => m_TargetColor;

        void Start()
        {
            m_Canvas = UIManager.Instance.Canvas;
        }


        public void OnDrag(PointerEventData eventData)
        {
            m_Crosshair.rectTransform.position += new Vector3(eventData.delta.x, eventData.delta.y);
        }


        public void SetTargetStatus(bool OnTarget)
        {
            if(OnTarget)
            {
                m_Crosshair.color = TargetColor;
            }
            else
            {
                m_Crosshair.color = DefaultColor;
            }
        }

        public void OnReset()
        {
            if(m_InitializeData == null)
                m_InitializeData = new Tuple<Vector3, Quaternion>(m_Crosshair.rectTransform.position, m_Crosshair.rectTransform.rotation);
            m_Crosshair.rectTransform.position = m_InitializeData.Item1;
            m_Crosshair.rectTransform.rotation = m_InitializeData.Item2;
        }
    }
}