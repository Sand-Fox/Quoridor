using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPathWall : BaseIA
{
    [SerializeField] private HorizontalWall horizontalWallPrefab;
    [SerializeField] private VerticalWall verticalWallPrefab;

    protected override void PlayIA()
    {
        if (UIManager.Instance.wallCount > 0)
        {
            Vector2 wallPosition = GetBestWallPosition(out Orientation orientation, out List<CustomTile> bestPathAfterWall);
            CustomWall wall;
            if (orientation == Orientation.Horizontal) wall = Instantiate(horizontalWallPrefab);
            else wall = Instantiate(verticalWallPrefab);
            wall.SetWall(wallPosition);
            UIManager.Instance.wallCount--;
            LinePopUp.Create(bestPathAfterWall, ColorExtension.green);
        }
    }


    private Vector2 GetBestWallPosition(out Orientation orientation, out List<CustomTile> bestPathAfterWall)
    {
        HorizontalWall horizontalWall = Instantiate(horizontalWallPrefab);
        VerticalWall verticalWall = Instantiate(verticalWallPrefab);
        List<CustomTile> playerBestPath = GetPlayerBestPath();
        foreach (CustomTile tile in playerBestPath) Debug.Log(tile);

        Vector2 bestWallPosition = default;
        orientation = default;
        bestPathAfterWall = new List<CustomTile>();

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

            if(corner1 != null)
            {
                wall.transform.position = corner1.transform.position;
                Debug.Log(corner1);
                if (wall.CanSpawnHere())
                {
                    wall.OnSpawn();
                    List<CustomTile> pathAfterWall = GetPlayerBestPath();
                    if (pathAfterWall.Count > bestPathAfterWall.Count)
                    {
                        bestWallPosition = wall.transform.position;
                        orientation = wall.orientation;
                        bestPathAfterWall = pathAfterWall;
                    }
                    wall.OnDespawn();
                }
            }

            if (corner2 != null)
            {
                wall.transform.position = corner2.transform.position;
                Debug.Log(corner2);
                if (wall.CanSpawnHere())
                {
                    wall.OnSpawn();
                    List<CustomTile> pathAfterWall = GetPlayerBestPath();
                    if (pathAfterWall.Count > bestPathAfterWall.Count)
                    {
                        bestWallPosition = wall.transform.position;
                        orientation = wall.orientation;
                        bestPathAfterWall = pathAfterWall;
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
