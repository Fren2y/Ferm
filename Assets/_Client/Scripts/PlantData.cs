using ferm;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Plant", order = 2)]
public class PlantData : ScriptableObject
{
    public Sprite plantIcon;

    public float plantingTime;
    public float growthTime;

    public int collectExp;
    public int collectCount;

    public CollectFruit collectFruit;
    public enum CollectFruit
    {
        none,
        carrot
    }

    public PlantModel plantModel;
}