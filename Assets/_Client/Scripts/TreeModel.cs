using UnityEngine;

namespace ferm
{
    public class TreeModel : PlantModel
    {
        [SerializeField] private Transform treeTr;

        public override void Planting(float progress)
        {
            treeTr.localScale = Vector3.one * 14 * Mathf.Clamp(0.1f + progress, 0.1f, 1.0f);
        }
    }
}
