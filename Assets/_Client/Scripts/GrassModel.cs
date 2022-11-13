using UnityEngine;

namespace ferm
{
    public class GrassModel : PlantModel
    {
        [SerializeField] private Transform[] grassTrs;

        public override void Planting(float progress)
        {
            for (int i = 0; i < grassTrs.Length; i++)
            {
                grassTrs[i].localScale = new Vector3(1, progress, 1);
            }
        }

    }
}
