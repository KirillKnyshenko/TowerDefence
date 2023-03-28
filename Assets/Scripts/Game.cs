using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;

    [SerializeField] private GameBoard _gameBoard;

    private void Start() {
        _gameBoard.Initialize(_boardSize);
    }

}
