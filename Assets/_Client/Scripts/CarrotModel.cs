using UnityEngine;

namespace ferm
{
    public class CarrotModel : PlantModel
    {
        [SerializeField] private GameObject[] cerrots;
        public override void Planting(float progress)
        {
            float progressPerCerrot = 1.0f / cerrots.Length;
            for (int i = 0; i < cerrots.Length; i++)
            {
                cerrots[i].SetActive(progress > progressPerCerrot * i);
            }
        }
    }
}
