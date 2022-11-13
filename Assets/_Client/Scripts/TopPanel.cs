using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace ferm
{
    public class TopPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI carrotsTmp;

        [SerializeField] private TextMeshProUGUI progressTmp;
        [SerializeField] private Slider progressSlider;

        private void Start()
        {
            GameLogic.Instance.e_updateCarrots += UpdateCarrots;
            GameLogic.Instance.e_updateProgress += UpdatePlayerProgress;
        }

        private void OnDestroy()
        {
            GameLogic.Instance.e_updateCarrots -= UpdateCarrots;
            GameLogic.Instance.e_updateProgress -= UpdatePlayerProgress;
        }

        private void UpdateCarrots(int count)
        {
            Debug.LogError($"Income: {count}");
            carrotsTmp.SetText(count.ToString("0"));
        }

        private void UpdatePlayerProgress(int exp, int req)
        {
            Debug.LogError($"Income: {exp} Req {req}");

            float progress = (float)exp / req;

            progressSlider.DOValue(progress, 0.5f).ChangeStartValue(progressSlider.value > progress ? 0.0f : progressSlider.value);

            float newExp = progressSlider.value > progress ? 0.0f : progressSlider.value * req;
            DOTween.To(() => newExp, x => newExp = x, exp, 0.5f).OnUpdate(() => 
            {
                progressTmp.SetText(newExp.ToString("0") + "/" + req.ToString());
            });
        }
    }
}
