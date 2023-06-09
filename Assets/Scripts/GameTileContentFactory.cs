using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class GameTileContentFactory : ScriptableObject
{
    [SerializeField] private GameTileContent _destinationPrefab;
    [SerializeField] private GameTileContent _emptyPrefab;
    [SerializeField] private GameTileContent _wallPrefab;
    public void Reclaim(GameTileContent content) {
        Destroy(content.gameObject);
    }

    public GameTileContent GetGameTileContent(GameTileContentType type) {
        switch (type)
        {
            case GameTileContentType.Destination:
                return Get(_destinationPrefab);
            case GameTileContentType.Empty:
                return Get(_emptyPrefab);
            case GameTileContentType.Wall:
                return Get(_wallPrefab);
        }

        return null;
    }

    public GameTileContent Get(GameTileContent prefab) {
        GameTileContent instance = Instantiate(prefab);
        instance.OriginalFactory = this;
        MoveToFactoryScene(instance.gameObject);
        return instance;
    }

    private Scene _contentScene;

    private void MoveToFactoryScene(GameObject gameObject) {
        if (!_contentScene.isLoaded)
        {
            if (Application.isEditor)
            {
                _contentScene = SceneManager.GetSceneByName(name);

                if (!_contentScene.isLoaded)
                {
                    _contentScene = SceneManager.CreateScene(name);           
                }
            } 
            else
            {
                _contentScene = SceneManager.CreateScene(name);
            }
        }

        SceneManager.MoveGameObjectToScene(gameObject, _contentScene);
    }
}
