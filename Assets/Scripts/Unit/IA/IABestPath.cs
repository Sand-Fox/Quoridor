using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABestPath : BaseIA
{
    protected override void PlayIA()
    {
        List<CustomTile> path = GetBestPath();
        if (path != null) SetUnit(path[1].transform.position);
    }
}
