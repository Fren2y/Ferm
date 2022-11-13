using System;
using System.Collections.Generic;
using Unity.AI.Navigation.Samples;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ferm
{
    public class FieldGenerator : MonoBehaviour
    {
        public int wight = 4;
        public int height = 4;

        [SerializeField] private Cell cellBase;

        [SerializeField] private LocalNavMeshBuilder meshBuilder;

        List<Cell> activeCells = new List<Cell>();

        public void GenerateField(Action<Vector3> fielGenerated)
        {
            float startSpawnW = -(float)(wight - 1);
            float startSpawnH = -(float)(height - 2);

            int cellID = 0;
            for (int w = 0; w < wight; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    Cell nCell = Instantiate(cellBase, new Vector3(startSpawnW + w * 2, 0, startSpawnH + h * 2), Quaternion.identity);

                    nCell.SetData(w, h, cellID);

                    activeCells.Add(nCell);
                }
            }

            meshBuilder.m_Size = new Vector3(wight * 2, 4, height * 2);

            meshBuilder.UpdateNavMesh();

            Vector3 playerPos = new Vector3(Random.Range(-wight, wight), 0, Random.Range(-height, height));

            fielGenerated(FindNavMeshPos(playerPos));
        }

        public void ClearField()
        {
            for (int i = 0; i < activeCells.Count; i++)
            {
                if (activeCells[i] != null)
                {
                    Destroy(activeCells[i].gameObject);
                }
            }

            activeCells.Clear();
        }

        public Vector3 FindPoisitionIndArea()
        {
            return FindNavMeshPos(new Vector3(Random.Range(-wight, wight), 0, Random.Range(-height, height))/2);
        }

        public Vector3 FindNavMeshPos(Vector3 pos)
        {
            Debug.DrawLine(pos, pos + Vector3.up * 10, Color.red, 1);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 10.0f, NavMesh.AllAreas))
            {
                Debug.DrawLine(hit.position, hit.position + Vector3.up * 10, Color.green, 1);
                return hit.position;
            }
            else
            {
                Debug.LogError("Cant Find Point On NavMesh");
                return Vector3.zero;
            }
        }
    }
}
