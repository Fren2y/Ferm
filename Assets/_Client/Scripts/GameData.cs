using System.Collections.Generic;
using UnityEngine;

namespace ferm
{
    public class GameData : MonoBehaviour
    {
        [SerializeField] List<PlantData> avaliblePlants;

        public PlantData[] GetAllPlants()
        {
            return avaliblePlants.ToArray();
        }

    }
}
