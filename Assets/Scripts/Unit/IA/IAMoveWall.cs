using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class IAMoveWall : BaseIA
{
    protected override void PlayIA()
    {
        if (wallCount > 0 && Random.value > 0.5)
        {
            Vector3 wallPosition = GetBestWallPosition(out Orientation orientation);
            GameObject wallObject = PhotonNetwork.Instantiate("Wall/" + orientation + "Wall", Vector3.zero, Quaternion.identity);
            wallObject.GetComponent<CustomWall>().view.RPC("SetWall", RpcTarget.All, wallPosition);
            wallCount--;
        }

        else
        {
            List<CustomTile> path = GetBestPath();
            if (path != null) SetUnit(path[1].transform.position);
        }
    }

    private Vector2 GetBestWallPosition(out Orientation orientation)
    {
        HorizontalWall horizontalWall = Instantiate(ReferenceManager.Instance.horizontalWallPrefab);
        VerticalWall verticalWall = Instantiate(ReferenceManager.Instance.verticalWallPrefab);
        List<CustomTile> playerBestPath = GetPlayerBestPath();

        Vector2 bestWallPosition = default; orientation = default;
        int longerPathCount = 0;

        for (int i = 0; i < playerBestPath.Count - 1; i++)
        {
            CustomTile currentTile = playerBestPath[i];
            CustomTile nextTile = playerBestPath[i + 1];
            Vector2 direction = nextTile.transform.position - currentTile.transform.position;

            CustomWall wall = null;
            if (direction == Vector2.up || direction == Vector2.down) wall = horizontalWall;
            if (direction == Vector2.right || direction == Vector2.left) wall = verticalWall;
            if (direction != Vector2.up && direction != Vector2.down && direction != Vector2.right && direction != Vector2.left) continue;

            CustomCorner corner1 = GridManager.Instance.GetCornerAtPosition((Vector2)currentTile.transform.position + 0.5f * direction + 0.5f * Vector2.Perpendicular(direction));
            CustomCorner corner2 = GridManager.Instance.GetCornerAtPosition((Vector2)currentTile.transform.position + 0.5f * direction - 0.5f * Vector2.Perpendicular(direction));

            if (corner1 != null)
            {
                wall.transform.position = corner1.transform.position;
                if (wall.CanSpawnHere())
                {
                    wall.OnSpawn();
                    List<CustomTile> pathAfterWall = GetPlayerBestPath();
                    if (pathAfterWall.Count > longerPathCount)
                    {
                        bestWallPosition = wall.transform.position;
                        orientation = wall.orientation;
                        longerPathCount = pathAfterWall.Count;
                    }
                    wall.OnDespawn();
                }
            }

            if (corner2 != null)
            {
                wall.transform.position = corner2.transform.position;
                if (wall.CanSpawnHere())
                {
                    wall.OnSpawn();
                    List<CustomTile> pathAfterWall = GetPlayerBestPath();
                    if (pathAfterWall.Count > longerPathCount)
                    {
                        bestWallPosition = wall.transform.position;
                        orientation = wall.orientation;
                        longerPathCount = pathAfterWall.Count;
                    }
                    wall.OnDespawn();
                }
            }
        }

        Destroy(horizontalWall.gameObject);
        Destroy(verticalWall.gameObject);
        return bestWallPosition;
    }

    
}
