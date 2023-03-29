using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameTile : MonoBehaviour
{
    [SerializeField] private Transform _arrowTransform;

    private GameTile _west, _east, _north, _south, _nextOnPath;

    private int _distance;

    public bool HasPath => _distance != int.MaxValue;
    public bool isAlternative { get; set; }
    
    private Quaternion _northRotation = Quaternion.Euler(90f, 0f, 0f);
    private Quaternion _eastRotation = Quaternion.Euler(90f, 90f, 0f);
    private Quaternion _southRotation = Quaternion.Euler(90f, 180f, 0f);
    private Quaternion _westRotation = Quaternion.Euler(90f, 270f, 0f);

    private GameTileContent _content;

    public GameTileContent Content
    {
        get => _content;
        set
        {
            if (_content != null)
            {
                _content.Recycle();
            }

            _content = value;
            _content.transform.localPosition = transform.localPosition;
        }
    }

    public static void MakeEastWestNeighbors(GameTile east, GameTile west) {
        // Static method 

        // Set east neighboor for west
        // Set west neighboor for east

        west._east = east;
        east._west = west;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south) {
        // Static method 

        // Set south neighboor for north
        // Set north neighboor for south

        north._south = south;
        south._north = north;
    }

    public void ClearPath() {
        _distance = int.MaxValue;
        _nextOnPath = null;
    }

    public void BecomeDestination() {
        // Make this Tile a destination

        _distance = 0;
        _nextOnPath = null;
    }

    private GameTile GrowPathTo(GameTile neighbor) {       
        if (!HasPath || neighbor == null || neighbor.HasPath)
        {
            return null;
        }

        // This Tile has a path
        // Argument neighbor is not null
        // Argument neighbor does not have a path

        // Set a path for argument neighbor
        // Set distance 1 more longer then this Tile has
        // Set next Tile on path this one

        neighbor._distance = _distance + 1;
        neighbor._nextOnPath = this;

        return neighbor.Content.Type != GameTileContentType.Wall ? neighbor : null;
    }

    // Do grow path by neighbor's relative position
    public GameTile GrowPathNorth() => GrowPathTo(_north);
    public GameTile GrowPathEast() => GrowPathTo(_east);
    public GameTile GrowPathSouth() => GrowPathTo(_south);
    public GameTile GrowPathWest() => GrowPathTo(_west);

    public void ShowPath() {
        if (_distance == 0)
        {
            // This is a destination Tile

            _arrowTransform.gameObject.SetActive(false);
            return;
        }

        // It is not a destination path
        // Show it's arrow
        // Rotate arrow to next on path Tile

        _arrowTransform.gameObject.SetActive(true);
        _arrowTransform.localRotation = 
            _nextOnPath == _north ? _northRotation :
            _nextOnPath == _east ? _eastRotation :
            _nextOnPath == _south ? _southRotation :
            _westRotation;
    }

}
