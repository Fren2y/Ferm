using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Cell", order = 1)]
public class CellData : ScriptableObject
{
    public string cellName;
    public CellType cellType;

    public enum CellType
    {
        grass,
        dirt,
        water
    }
}