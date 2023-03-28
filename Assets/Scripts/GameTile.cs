using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField] private Transform _arrowTransform;
    private GameTile _west, _east, _north, _south, _nextOnPath;
    private int _distance;
    public bool HasPath => _distance != int.MaxValue;
    public static void MakeEastWestNeighbors(GameTile east, GameTile west) {
        east._east = east;
        west._west = west;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south) {
        north._north = north;
        south._south = south;
    }

    public void ClearPath() {
        _distance = int.MaxValue;
        _nextOnPath = null;
    }

    public void BecomeDestination() {
        _distance = 0;
        _nextOnPath = null;
    }
}
