using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ferm
{
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager Instance;

        [SerializeField] private TopPanel topPanel;
        [SerializeField] private BotPanel botPanel;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void ShowCellInfo(CellData cellData)
        {
            botPanel.Activate(cellData);
        }

        public void HideBotPanel()
        {
            botPanel.ClosePlantingPanel();
        }
    }
}
