using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

namespace ferm
{
    public class Cell : MonoBehaviour, IClickable
    {
        public bool cellBusy { get; private set; }

        [SerializeField] private Transform growthPos;
        [SerializeField] private CellData cellData;
        [SerializeField] private TextMeshPro timerTmp;

        [SerializeField] private MeshRenderer cellMeshRendere;
        public PlantModel planlingObj { get; private set; }
        public PlantData plantData { get; private set; }

        private Tween growTween;

        public int posX { get; private set; }
        public int posY { get; private set; }
        public int cellID { get; private set; }
        public bool isBusy { get; private set; }
        public bool isReady { get; private set; }

        public void SetData(int x, int y, int id)
        {
            posX = x;
            posY = y;
            cellID = id;
        }

        public void Click()
        {
            Debug.LogError($"Click Cell {cellData.cellName} Type {cellData.cellType} Busy {isBusy} Ready {isReady}" );

            if (isReady)
            {
                StartCoroutine(GameLogic.Instance.Collect(this));
                return;
            }

            if (!isBusy)
            {
                GUIManager.Instance.ShowCellInfo(cellData);
                cellMeshRendere.material.color = new Color(0.5699763f, 0.745283f, 0.2144446f);
            }  
        }

        public void Diselect()
        {
            cellMeshRendere.material.color = new Color(0.3699763f, 0.745283f, 0.2144446f);
        }

        public void Planting(PlantData data)
        {
            plantData = data;

            planlingObj = Instantiate(data.plantModel, growthPos);
            planlingObj.transform.localPosition = Vector3.zero;
            planlingObj.transform.localEulerAngles = Vector3.zero * Random.Range(0.0f, 360.0f);

            growTween?.Kill();

            timerTmp.transform.rotation = Camera.main.transform.rotation;

            float growT = 0.0f;

            DOTween.To(() => growT, x => growT = x, 1.0f, data.growthTime).SetEase(Ease.Linear).OnUpdate(() =>
            {
                planlingObj.Planting(growT);
                timerTmp.SetText((data.growthTime - (data.growthTime * growT)).ToString("0.0") + "s");
            }).OnComplete(() =>
            {
                timerTmp.SetText("<color=#77BF81><size=200%>V");
                isBusy = false;
                isReady = true;
            });

            isBusy = true;
        }

        public void Collect()
        {
            timerTmp.SetText("");
            Debug.LogError("Collecting Destroy Plant");
            Destroy(planlingObj.gameObject);

            isBusy = false;
            isReady = false;
        }
    }
}
