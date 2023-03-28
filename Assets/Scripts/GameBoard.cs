using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Transform _groundTransform;
    private Vector2Int _size;
    [SerializeField] private GameTile _gameTilePrefab;
    private GameTile[] _gameTilesArray;
    public void Initialize(Vector2Int size) {
        _size = size;
        _groundTransform.localScale = new Vector3(size.x, size.y, 1f);

        Vector3 gameTileOffset = new Vector3((size.x - 1) * 0.5f, 0, (size.y - 1) * 0.5f);

        _gameTilesArray = new GameTile[size.x * size.y];
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile gameTile = _gameTilesArray[i] = Instantiate(_gameTilePrefab);
                gameTile.transform.SetParent(transform, false);
                gameTile.transform.localPosition = new Vector3(x - gameTileOffset.x, 0, y - gameTileOffset.z);
            
                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(gameTile, _gameTilesArray[i - 1]);
                }

                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(gameTile, _gameTilesArray[i - size.x]);
                }
            }
        }
    }
}
