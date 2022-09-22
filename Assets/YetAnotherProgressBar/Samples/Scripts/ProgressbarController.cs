﻿using UnityEngine;
using System.Collections;
using System;

namespace YAProgressBar
{
    public class ProgressbarController : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarBaseMesh[] bars;

        private int currentBar = 0;
        private float currentBarStartTime;
        private float currentBarEndTime;
        private float barUpdateDuration = 3f;

        void Start()
        {
            currentBar = 0;
            currentBarStartTime = Time.time;
            currentBarEndTime = currentBarStartTime + barUpdateDuration;
            ResetBars();
        }

        void Update()
        {
            if (currentBar < bars.Length)
            {
                bars[currentBar].FillAmount = Mathf.InverseLerp(currentBarStartTime, currentBarEndTime, Time.time);
                if (bars[currentBar].FillAmount == 1)
                {
                    currentBar++;
                    if (currentBar == bars.Length)
                    {
                        currentBar = 0;
                        ResetBars();
                    }
                    currentBarStartTime = Time.time;
                    currentBarEndTime = currentBarStartTime + barUpdateDuration;
                }
            }
        }

        private void ResetBars()
        {
            for (int i = 0; i < bars.Length; i++)
            {
                bars[i].FillAmount = 0;
            }
        }
    }
}
