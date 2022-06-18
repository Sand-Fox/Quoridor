using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager Instance;

    private void Awake() => Instance = this;

    [Header("In Game")]
    public Player player;
    public BaseUnit enemy;

    [Header("Resources Prefabs")]
    public HorizontalWall horizontalWallPrefab;
    public VerticalWall verticalWallPrefab;
    public IAMove IAMove;
    public IAWall IAWall;
    public IAMoveWall IAMoveWall;
}
