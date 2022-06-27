using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMove : BaseIA
{
    public static string description = "IA qui se déplace en utilisant \n l'algorithme A*";

    protected override void PlayIA()
    {
        List<CustomTile> path = PathFinding.Instance.GetWiningPath(this);
        if (path.Count != 0) SetUnit(path[0].transform.position);
    }
}
