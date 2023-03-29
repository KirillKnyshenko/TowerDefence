using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;

    [SerializeField] private GameBoard _gameBoard;

    [SerializeField] private Camera _camera;
    [SerializeField] private GameTileContentFactory _contentFactory;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private void Start() {
        _gameBoard.Initialize(_boardSize, _contentFactory);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            HandleAlternativeTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleTouch();
        }
    }

    private void HandleTouch() {
        GameTile gameTile = _gameBoard.GetGameTile(TouchRay);

        if (gameTile != null)
        {
            _gameBoard.ToggleDestination(gameTile);
        }
    }

    private void HandleAlternativeTouch() {
        GameTile gameTile = _gameBoard.GetGameTile(TouchRay);
        
        if (gameTile != null)
        {
            _gameBoard.ToggleWall(gameTile);
        }
    }
}
