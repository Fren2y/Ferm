using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;
using TMPro;
using DG.Tweening;

namespace ferm
{
    public class Player : MonoBehaviour, IClickable
    {
        [SerializeField] private Animator anim;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private TextMeshPro timerTmp;
        public bool haveTask { get; private set; }

        private int speedHash;
        private int actionHash;

        private Tween actionTween;

        private void Awake()
        {
            speedHash = Animator.StringToHash("Speed");
            actionHash = Animator.StringToHash("Action");
        }

        public void ActivatePlayer(Vector3 pos)
        {
            agent.Warp(pos);
            StartCoroutine(AnimationLogik());
        }

        public void Click()
        {
            Debug.LogError("Click Player");
        }

        public void Diselect()
        {

        }

        private void Update()
        {
            anim.SetFloat(speedHash, agent.velocity.magnitude);
        }

        public void StopCurrentTask()
        {
            StopAllCoroutines();
            actionTween?.Kill();
            anim.SetBool(actionHash, false);
            timerTmp.SetText("");
        }

        public IEnumerator SetDestination(Action<bool> res, Vector3 pos)
        {
            haveTask = true;

            if (agent.SetDestination(pos))
            {
                Debug.Log("Have Task");
            }
            else
            {
                Debug.LogError("Cant Find Path");

                haveTask = false;

                res(false);
                yield break;
            }

            yield return null;

            while (agent.remainingDistance > agent.stoppingDistance)
            {
                Debug.LogWarning("W8");
                yield return null;
            }

            haveTask = false;

            res(true);
        }

        public IEnumerator Planting(Action<bool> res, PlantData data, Cell cellData)
        {
            haveTask = true;

            anim.SetBool(actionHash, true);

            timerTmp.transform.rotation = Camera.main.transform.rotation;

            float timer = data.plantingTime;
            actionTween = DOTween.To(() => timer, x => timer = x, 0, data.plantingTime).SetEase(Ease.Linear).OnUpdate(()=>
            {
                timerTmp.SetText(timer.ToString("0.0") + "s");
               
            }).OnComplete(() =>
            {
                timerTmp.SetText("");
            });

            yield return new WaitForSeconds(data.plantingTime);

            anim.SetBool(actionHash, false);

            haveTask = false;

            res(true);
        }

        public IEnumerator Collect(Action<bool> res, PlantData pData, Cell cell)
        {
            yield return null;

            res(true);
        }

        private IEnumerator AnimationLogik()
        {
            yield break;

            for (;;)
            {
                while (haveTask)
                {
                    yield return new WaitForSeconds(0.5f);
                }

                bool moveTo = UnityEngine.Random.Range(0.0f, 1.0f) > 0.75f;

                if (moveTo)
                {
                    agent.SetDestination(GameLogic.Instance.fieldGenerator.FindPoisitionIndArea());
                }

                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
