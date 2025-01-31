using System;
using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : MonoBehaviour
{
    [SerializeField] private Transform _markerCirclePrefab;
    [SerializeField] private Transform _markerTrianglePrefab;
    private const float GRID_SIZE = 3f;
    private void Start()
    {
        GameManager.Instance.OnClickGridPosition += GameManager_OnClickGridPosition;
    }

    private void GameManager_OnClickGridPosition(object sender, GameManager.OnClickGridPositionEventArgs e)
    {
        Transform spawnedMarker = Instantiate(_markerCirclePrefab);
        
        if(spawnedMarker.TryGetComponent(out NetworkObject newMarker)) newMarker.Spawn(true);
        spawnedMarker.position = GetGridWorldPosition(e.x, e.y);
    }

    private Vector2 GetGridWorldPosition(int x, int y)
    {
        return new Vector2(-GRID_SIZE + x * GRID_SIZE, -GRID_SIZE + y * GRID_SIZE);
    }
}
