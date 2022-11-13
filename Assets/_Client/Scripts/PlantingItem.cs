using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ferm
{
    public class PlantingItem : MonoBehaviour
    {
        [SerializeField] private Image iconImg;
        [SerializeField] private Button plantBtn;

        private PlantData _plantData;

        internal void LoadInfo(PlantData plantData)
        {
            _plantData = plantData;

            iconImg.sprite = plantData.plantIcon;

            plantBtn.onClick.RemoveAllListeners();
            plantBtn.onClick.AddListener(UsePlantBtn);
        }

        private void UsePlantBtn()
        {
            Debug.Log("Press Btn");
            StartCoroutine(GameLogic.Instance.UsePlant(_plantData));
        }
    }
}
