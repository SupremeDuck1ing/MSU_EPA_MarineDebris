﻿using UnityEngine;
using System.Collections;
using System;

namespace YAProgressBar
{
    public class ProgressbarAmmo : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarBaseMesh ammoBar;
        [SerializeField]
        private ProgressBarBaseMesh temperatureBar;

        [SerializeField]
        private int ammoCount = 32;
        [SerializeField]
        private int ammoMax = 32;

        private float currentBarStartTime;
        private float currentBarEndTime;
        private float decreaseTime;
        private float temperature = 0;
        private float temperatureMax = 120f;
        private float temperatureIncrement = 4;
        private float temperatureDecrement = 1;
        private float cold = 1;
        private bool canProduce;
        private float nextProduceTime = 0;
        private float produceDelay = 0.3f;
        private float nextCoolingTime = 0;
        private float coolingDelay = 0.1f;
        private float prevDecreaseAmmoTime;

        public int AmmoCount
        {
            get { return ammoCount; }
        }

        public void DecreaseAmmo()
        {
            ammoCount--;
            UpdateAmmoBar();
            if (temperature < temperatureMax)
            {
                temperature += temperatureIncrement * (1f / (Time.time - prevDecreaseAmmoTime));
                UpdateTemperatureBar();
            }

            prevDecreaseAmmoTime = Time.time;
        }

        private void UpdateAmmoBar()
        {
            ammoBar.FillAmount = Mathf.InverseLerp(0, ammoMax, ammoCount);
        }

        private void UpdateTemperatureBar()
        {
            temperatureBar.FillAmount = Mathf.InverseLerp(0, temperatureMax, temperature);
        }

        void Start()
        {
            ReloadAmmo();
            UpdateTemperatureBar();
            StartCoroutine(ProduceAmmo());
        }

        private IEnumerator ProduceAmmo()
        {
            while (true)
            {
                if (cold <= temperature && nextCoolingTime < Time.time)
                {
                    temperature -= temperatureDecrement * (Time.time - prevDecreaseAmmoTime);
                    UpdateTemperatureBar();
                    nextCoolingTime = Time.time + coolingDelay;
                }

                yield return null;
                if (ammoCount < ammoMax && temperature < cold && canProduce)
                {
                    ammoCount++;
                    UpdateAmmoBar();
                    canProduce = false;
                    nextProduceTime = Time.time + produceDelay;
                }

                yield return null;

                if (nextProduceTime < Time.time)
                {
                    canProduce = true;
                }
            }
        }

        //void Update()
        //{
        //    ReloadAmmo();
        //}

        private void ReloadAmmo()
        {
            ammoCount = ammoMax;
            ammoBar.FillAmount = Mathf.InverseLerp(0, ammoMax, ammoCount);
        }
    }
}
