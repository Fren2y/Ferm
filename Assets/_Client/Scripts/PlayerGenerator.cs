using System;
using UnityEngine;

namespace ferm
{
    public class PlayerGenerator : MonoBehaviour
    {
        [SerializeField] private Player player;

        public void GeneratePlayer(Action<Player> res, Vector3 pos)
        {
            Player pl = Instantiate(player, pos, Quaternion.identity);
            pl.ActivatePlayer(pos);
            res(pl);
        }
    }
}
