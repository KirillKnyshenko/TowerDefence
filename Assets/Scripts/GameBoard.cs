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
    private GameTileContentFactory _contentFactory;
    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory) {
        _size = size;
        _groundTransform.localScale = new Vector3(size.x, size.y, 1f);

        // Array of Tiles will spawn in the center of Ground
        // Make offset for array of Tiles
        // The center of the first point = (Size - 1) * 0.5
        
        Vector3 gameTileOffset = new Vector3((size.x - 1) * 0.5f, 0, (size.y - 1) * 0.5f);

        _gameTilesArray = new GameTile[size.x * size.y];
        _contentFactory = contentFactory;
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

                gameTile.Content = _contentFactory.GetGameTileContent(GameTileContentType.Empty);
            }
        }

        ToggleDestination(_gameTilesArray[_gameTilesArray.Length / 2]);
    }

    public bool FindPath() {
        foreach (var gameTile in _gameTilesArray)
        {
            if (gameTile.Content.Type == GameTileContentType.Destination)
            {
                gameTile.BecomeDestination();
                _searchFrontier.Enqueue(gameTile);
            }
            else
            {
                gameTile.ClearPath();
            }
        }

        if (_searchFrontier.Count == 0)
        {
            return false;
        }
        
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
            if (!gameTile.HasPath)
            {
                return false;
            }
        }

        foreach (var gameTile in _gameTilesArray)
        {
            // Show Tiles' arrows
            gameTile.ShowPath();
        }

        return true;
    }

    public void ToggleDestination(GameTile gameTile) {
        if (gameTile.Content.Type == GameTileContentType.Destination)
        {
            gameTile.Content = _contentFactory.GetGameTileContent(GameTileContentType.Empty);

            if (!FindPath())
            {
                gameTile.Content = _contentFactory.GetGameTileContent(GameTileContentType.Destination);
                FindPath();
            }            
        }
        else if (gameTile.Content.Type == GameTileContentType.Empty)
        {
            gameTile.Content = _contentFactory.GetGameTileContent(GameTileContentType.Destination);
            FindPath();
        }
    }

    public void ToggleWall(GameTile gameTile) {
        if (gameTile.Content.Type == GameTileContentType.Wall)
        {
            gameTile.Content = _contentFactory.GetGameTileContent(GameTileContentType.Empty);

            FindPath();          
        }
        else if (gameTile.Content.Type == GameTileContentType.Empty)
        {
            gameTile.Content = _contentFactory.GetGameTileContent(GameTileContentType.Wall);

            if (!FindPath())
            {
                gameTile.Content = _contentFactory.GetGameTileContent(GameTileContentType.Empty);
                FindPath();
            }  
        }
    }

    public GameTile GetGameTile(Ray ray) {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            int x = (int) (hit.point.x + _size.x * 0.5f);
            int y = (int) (hit.point.z + _size.y * 0.5f);

            if (x >= 0 && x < _size.x && y >= 0 && x < _size.y)
            {
                return _gameTilesArray[x + (_size.x * y)];
            }
        }

        return null;
    }
}
