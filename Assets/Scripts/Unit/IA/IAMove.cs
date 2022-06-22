using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMove : BaseIA
{
    public static string description = "IA qui se déplace uniquement";

    protected override void PlayIA()
    {
        List<CustomTile> path = GetBestPath();
        if (path.Count != 0) SetUnit(path[1].transform.position);
    }
}
