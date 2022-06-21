using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MiniMaxIA : BaseIA
{

    protected override void PlayIA()
    {

    }


    private void CalculScore(Node node)
        {
            // recuperer la distance de la fin de l'IA et celle du joueur
            List<CustomTile> pathIA = GetBestPath();
            List<CustomTile> pathP = GetPlayerBestPath(); 
            
            // recuperer le nombre de mur restant a l'IA et celle du joueur
            int nbWallIA = this.wallCount;
            int nbWallP = UIManager.Instance.wallCount;
            // fonction du type f(nbMurIA, distIA, nbMurJ, distJ) = nbMurIA * 2 + distIA - nbMurJ * 2 - distJ       -> a voir si pertinent 
            int score = 0;
            node.setScore(score);
        }
}