using System;
using System.Collections;
using UnityEngine;

namespace ferm
{
    public class GameLogic : MonoBehaviour
    {
        public static GameLogic Instance;

        public Action e_gameReady;
        public Action<int> e_updateCarrots;
        public Action<int, int> e_updateProgress;

        public FieldGenerator fieldGenerator { get; private set; }
        public PlayerGenerator playerGenerator { get; private set; }
        public GameData gameData { get; private set; }
        public Player activePlayer { get; private set; }

        private IClickable _activeObject;
        private Cell _activeCell;
        private Coroutine _actionCor;

        private int carrotCount = 0;
        private int expCount = 0;
        private int level = 1;

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

            fieldGenerator = FindObjectOfType<FieldGenerator>();
            playerGenerator = FindObjectOfType<PlayerGenerator>();
            gameData = FindObjectOfType<GameData>();
        }

        private void Start()
        {
            fieldGenerator.GenerateField((z) =>
            {
                playerGenerator.GeneratePlayer((p) =>
                {
                    activePlayer = p;
                    e_gameReady?.Invoke();


                    e_updateCarrots?.Invoke(0);
                    e_updateProgress?.Invoke(0, 1);

                }, z);
            });
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.TryGetComponent(out Cell cell))
                    {
                        Diselect();

                        cell.Click();

                        _activeCell = cell;
                        _activeObject = cell.GetComponent<IClickable>();
                        return;
                    }
                }
            }
        }

        public void Diselect()
        {
            if (_activeObject != null)
            {
                _activeObject.Diselect();
                _activeObject = null;
            }

            if (_activeCell != null)
            {
                _activeCell = null;
            }
        }

        internal IEnumerator UsePlant(PlantData plantData)
        {
            if (_activeCell == null) { Debug.LogError("Active Cell == NULL"); yield break; }
            if (_activeCell.isBusy) { Debug.LogError("Cell BUSY"); yield break; }

            activePlayer.StopCurrentTask();

            Cell aCell = _activeCell;

            if (_actionCor != null) StopCoroutine(_actionCor);

            GUIManager.Instance.HideBotPanel();
            aCell.Diselect();
            Diselect();

            _actionCor = StartCoroutine(activePlayer.SetDestination((r) =>
            {
                if (r)
                {
                    _actionCor = StartCoroutine(activePlayer.Planting((r) =>
                    {
                        if (r) aCell.Planting(plantData);
                        else
                        {
                            Debug.LogError("Error Planting");
                        }
                    }, plantData, aCell));
                }
                else
                {
                    Debug.LogError("Error Find Path");
                    return;
                }

            }, fieldGenerator.FindNavMeshPos(aCell.transform.position)));
        }

        public IEnumerator Collect(Cell cell)
        {
            if (cell.plantData.collectFruit == PlantData.CollectFruit.none && cell.plantData.collectExp <= 0)
            {
                Debug.LogError("Cant Collect");
                yield break;
            }

            activePlayer.StopCurrentTask();

            if (_actionCor != null) StopCoroutine(_actionCor);

            Diselect();

            _actionCor = StartCoroutine(activePlayer.SetDestination((r) =>
            {
                if (r)
                {
                    AddFruit(cell.plantData.collectFruit, cell.plantData.collectCount);
                    AddExp(cell.plantData.collectExp);
                    cell.Collect();
                }
                else
                {
                    Debug.Log("Error Find Path");
                    return;
                }

            }, fieldGenerator.FindNavMeshPos(cell.transform.position)));
        }

        public void AddFruit(PlantData.CollectFruit item, int count)
        {
            if (count <= 0) return;
            if (item == PlantData.CollectFruit.none) return;
            Debug.Log($"Collect {item} Count {count}");

            switch (item)
            {
                case PlantData.CollectFruit.carrot:
                    
                    carrotCount += count;
                    e_updateCarrots?.Invoke(carrotCount);
                    break;
            }

           
        }

        public void AddExp(int count)
        {
            if (count <= 0) return;

            Debug.Log($"Add Exp {count}");
            expCount += count;

            while (expCount >= level * 500)
            {
                expCount -= level * 500;
                level++;
            }

            e_updateProgress?.Invoke(expCount, level * 500);
        }
    }
}
