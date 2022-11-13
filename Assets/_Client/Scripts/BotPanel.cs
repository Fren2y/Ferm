using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace ferm
{
    public class BotPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform panelRT;
        [SerializeField] private Button closeBtn;

        [SerializeField] private Transform btnsRoot;

        [SerializeField] private PlantingItem plantingItemBase;

        private PlantingItem[] plantsBtns;

        private void Start()
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.AddListener(ClosePlantingPanel);

            //Instantiate Plants Btn
            PlantData[] allPlantsData = GameLogic.Instance.gameData.GetAllPlants();
            plantsBtns = new PlantingItem[allPlantsData.Length];
            for (int i = 0; i < allPlantsData.Length; i++)
            {
                plantsBtns[i] = Instantiate(plantingItemBase, btnsRoot);
                plantsBtns[i].LoadInfo(allPlantsData[i]);
            }
        }

        internal void Activate(CellData cellData)
        {
            panelRT.DOAnchorPosY(0.0f, 0.5f);
        }

        public void ClosePlantingPanel()
        {
            panelRT.DOAnchorPosY(-panelRT.sizeDelta.y, 0.5f);
            GameLogic.Instance.Diselect();
        }
    }
}
