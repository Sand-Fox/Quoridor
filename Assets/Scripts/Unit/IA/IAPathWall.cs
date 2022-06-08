using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPathWall : BaseIA
{
    [SerializeField] private HorizontalWall horizontalWallPrefab;
    [SerializeField] private VerticalWall verticalWallPrefab;

    protected override void PlayIA()
    {
        
    }


    private Vector2 GetBestWallPosition(out List<CustomTile> bestPathAfterWall)
    {
        HorizontalWall horizontalWall = Instantiate(horizontalWallPrefab);
        VerticalWall verticalWall = Instantiate(verticalWallPrefab);
        List<CustomTile> playerBestPath = GetPlayerBestPath();

        Vector2 bestWallPosition = default;
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

            wall.transform.position = corner1.transform.position;
            if (wall.CanSpawnHere())
            {
                wall.OnSpawn();
                List<CustomTile> pathAfterWall = GetPlayerBestPath();
                if(pathAfterWall.Count > bestPathAfterWall.Count)
                {
                    bestWallPosition = wall.transform.position;
                    bestPathAfterWall = pathAfterWall;
                }
                wall.OnDespawn();
            }

            wall.transform.position = corner2.transform.position;
            if (wall.CanSpawnHere())
            {
                wall.OnSpawn();
                List<CustomTile> pathAfterWall = GetPlayerBestPath();
                if (pathAfterWall.Count > bestPathAfterWall.Count)
                {
                    bestWallPosition = wall.transform.position;
                    bestPathAfterWall = pathAfterWall;
                }
                wall.OnDespawn();
            }
        }

        return bestWallPosition;
    }

    
}
