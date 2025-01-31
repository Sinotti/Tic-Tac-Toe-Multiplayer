using System.Linq.Expressions;
using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : NetworkBehaviour
{
    [SerializeField] private Transform _markerCirclePrefab;
    [SerializeField] private Transform _markerTrianglePrefab;
    
    private const float GRID_SIZE = 3f;
    private Transform _currentPrefab;
    private void Start()
    {
        GameManager.Instance.OnClickGridPosition += GameManager_OnClickGridPosition;
    }

    private void GameManager_OnClickGridPosition(object sender, GameManager.OnClickGridPositionEventArgs e)
    {
        Debug.Log("Local_GameManager_SpawnObject");
        SpawnObjectRpc(e.x, e.y, e.playerType);
    }

    [Rpc(SendTo.Server)] // Send a message to server and the server runs this code in server
    private void SpawnObjectRpc(int x, int y, GameManager.PlayerType playerType)
    {
        Debug.Log("Server_SpawnObject");
        
        Transform spawnedMarker = Instantiate(SetPlayerType(playerType), GetGridWorldPosition(x, y), Quaternion.identity);
        if(spawnedMarker.TryGetComponent(out NetworkObject newMarker)) newMarker.Spawn(true);
    }
    
    private Vector2 GetGridWorldPosition(int x, int y)
    {
        return new Vector2(-GRID_SIZE + x * GRID_SIZE, -GRID_SIZE + y * GRID_SIZE);
    }

    private Transform SetPlayerType(GameManager.PlayerType playerType)
    {
        Transform prefab;
        switch (playerType)
        {
            default:
            case GameManager.PlayerType.Circle:
                prefab = _markerCirclePrefab;
                break;
            case GameManager.PlayerType.Triangle:
                prefab = _markerTrianglePrefab;
                break;
        }

        return prefab;
    }
}
