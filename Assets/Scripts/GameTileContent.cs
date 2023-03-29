using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileContent : MonoBehaviour
{
    [SerializeField] private GameTileContentType _type;
    public GameTileContentType Type => _type;
    public GameTileContentFactory OriginalFactory { get; set; }

    public void Recycle() {
        OriginalFactory.Reclaim(this);
    }
}

public enum GameTileContentType {
    Empty,
    Destination,
    Wall
}
