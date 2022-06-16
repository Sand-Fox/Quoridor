using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager Instance;

    private void Awake() => Instance = this;

    public Player player;
    public BaseUnit enemy;

    public HorizontalWall horizontalWallPrefab;
    public VerticalWall verticalWallPrefab;

}
