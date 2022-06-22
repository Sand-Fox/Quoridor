using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private GameObject tilesFolder;
    [SerializeField] private GameObject cornersFolder;
    [SerializeField] private CustomTile tilePrefab;
    [SerializeField] private CustomCorner cornerPrefab;

    public static readonly int BOUNDS = 9;
    public static readonly int MAXPATH = BOUNDS * BOUNDS;
    private Dictionary<Vector2, CustomTile> tilesDico = new Dictionary<Vector2, CustomTile>();
    public Dictionary<Vector2, CustomCorner> cornersDico = new Dictionary<Vector2, CustomCorner>();

    [HideInInspector] public CustomCorner selectedCorner;

    void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += OnGameStateChanged;
        ModeManager.OnModeChanged += OnModeChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        ModeManager.OnModeChanged -= OnModeChanged;
    }

    private void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.GenerateGrid) GenerateGrid();
    }

    private void OnModeChanged(Mode newMode) => ResetAllTiles();

    public void GenerateGrid()
    {
        Camera.main.transform.position = new Vector3((BOUNDS - 1)/ 2, (BOUNDS - 1)/ 2, Camera.main.transform.position.z);

        for (int x = 0; x < BOUNDS; x++)
        {
            for (int y = 0; y < BOUNDS; y++)
            {
                CustomTile spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity, tilesFolder.transform);
                spawnedTile.name = $"Tile {x} {y}";
                tilesDico[new Vector2(x, y)] = spawnedTile;
            }
        }

        for (int x = 0; x < BOUNDS - 1; x++)
        {
            for (int y = 0; y < BOUNDS - 1; y++)
            {
                CustomCorner spawnedCorner = Instantiate(cornerPrefab, new Vector3(x + 0.5f, y + 0.5f), Quaternion.identity, cornersFolder.transform);
                spawnedCorner.name = $"Corner {x + 0.5f} {y + 0.5f}";
                cornersDico[new Vector2(x + 0.5f, y + 0.5f)] = spawnedCorner;
            }
        }
    }

    public CustomTile GetTileAtPosition(Vector2 pos)
    {
        if (tilesDico.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public CustomCorner GetCornerAtPosition(Vector2 pos)
    {
        if (cornersDico.TryGetValue(pos, out var corner)) return corner;
        return null;
    }

    public CustomTile[] GetFirstRaw()
    {
        CustomTile[] firstRaw = new CustomTile[BOUNDS];
        for (int i = 0; i < BOUNDS; i++) firstRaw[i] = GetTileAtPosition(new Vector2(i, 0));
        return firstRaw;
    }

    public CustomTile[] GetLastRaw()
    {
        CustomTile[] lastRaw = new CustomTile[BOUNDS];
        for (int i = 0; i < BOUNDS; i++) lastRaw[i] = GetTileAtPosition(new Vector2(i, BOUNDS - 1));
        return lastRaw;
    }

    public void ResetAllTiles()
    {
        foreach (KeyValuePair<Vector2, CustomTile> pair in tilesDico)
        {
            pair.Value.EnableVisual(false);
            pair.Value.previousTile = null;
            pair.Value.G = MAXPATH;
            pair.Value.H = MAXPATH;
        }
    }

    public void ResetAllCorners()
    {
        foreach (KeyValuePair<Vector2, CustomCorner> pair in cornersDico)
        {
            pair.Value.EnableVisual(false);
        }
    }
}
