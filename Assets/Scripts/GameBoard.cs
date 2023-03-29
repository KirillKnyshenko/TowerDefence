using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Transform _groundTransform;
    private Vector2Int _size;
    [SerializeField] private GameTile _gameTilePrefab;
    private GameTile[] _gameTilesArray;
    private Queue<GameTile> _searchFrontier = new Queue<GameTile>();

    public void Initialize(Vector2Int size) {
        _size = size;
        _groundTransform.localScale = new Vector3(size.x, size.y, 1f);

        // Array of Tiles will spawn in the center of Ground
        // Make offset for array of Tiles
        // The center of the first point = (Size - 1) * 0.5
        
        Vector3 gameTileOffset = new Vector3((size.x - 1) * 0.5f, 0, (size.y - 1) * 0.5f);

        _gameTilesArray = new GameTile[size.x * size.y];

        // I will be an index for one dementinal array of Tiles
        // X,Y will be used for coordinates and position in space

        //      ^   //          //
        //  Y   |   //  X -->   //  I - number
        //          //          //

        // Generate Tiles

        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile gameTile = _gameTilesArray[i] = Instantiate(_gameTilePrefab);
                gameTile.transform.SetParent(transform, false);
                gameTile.transform.localPosition = new Vector3(x - gameTileOffset.x, 0, y - gameTileOffset.z);
            
                if (x > 0)
                {
                    // West neighbor is
                    GameTile.MakeEastWestNeighbors(gameTile, _gameTilesArray[i - 1]);
                }

                if (y > 0)
                {
                    // South neighbor is
                    GameTile.MakeNorthSouthNeighbors(gameTile, _gameTilesArray[i - size.x]);
                }


                // Separate even and odd
                gameTile.isAlternative = (x & 1) == 0;
                
                //Invert it in every row
                if ((y & 1) == 0)
                {
                    gameTile.isAlternative = !gameTile.isAlternative;
                }
            }
        }

        FindPath();
    }

    public void FindPath() {
        foreach (var gameTile in _gameTilesArray)
        {
            gameTile.ClearPath();
        }

        int destinathionIndex = _gameTilesArray.Length / 2;

        _gameTilesArray[destinathionIndex].BecomeDestination();

        // Push a destination in queue
        _searchFrontier.Enqueue(_gameTilesArray[destinathionIndex]);
        
        while (_searchFrontier.Count > 0)
        {
            GameTile gameTile = _searchFrontier.Dequeue();

            if (gameTile != null)
            {
                // Queue order depends on is Tile alternative

                if (gameTile.isAlternative)
                {
                    _searchFrontier.Enqueue(gameTile.GrowPathNorth());
                    _searchFrontier.Enqueue(gameTile.GrowPathSouth());
                    _searchFrontier.Enqueue(gameTile.GrowPathEast());
                    _searchFrontier.Enqueue(gameTile.GrowPathWest());    
                }
                else 
                {
                    _searchFrontier.Enqueue(gameTile.GrowPathWest());   
                    _searchFrontier.Enqueue(gameTile.GrowPathEast());
                    _searchFrontier.Enqueue(gameTile.GrowPathSouth());
                    _searchFrontier.Enqueue(gameTile.GrowPathNorth());
                }            
            }
        }

        foreach (var gameTile in _gameTilesArray)
        {
            // Show Tiles' arrows
            gameTile.ShowPath();
        }
    }
}
