using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public EventHandler<OnClickGridPositionEventArgs> OnClickGridPosition;

    private PlayerType _localPlayerType;
    private PlayerType _currentPlayerTurn;
    
    public class OnClickGridPositionEventArgs : EventArgs
    {
        public int x;
        public int y;
        public PlayerType playerType;
    }

    public enum PlayerType
    {
        None,
        Circle,
        Triangle,
    }
    
    private void Awake()
    {
        if(Instance != null) Destroy(this);
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("OnNetworkSpawn" + NetworkManager.Singleton.LocalClientId);
        
        if(NetworkManager.Singleton.LocalClientId == 0) _localPlayerType = PlayerType.Circle;
        else _localPlayerType = PlayerType.Triangle;
        
        // What really matters is the server authority
        if(IsServer) _currentPlayerTurn = PlayerType.Circle;
    }

    [Rpc(SendTo.Server)]
    public void ClickedOnGridPositionRpc(int x, int y, PlayerType playerType)
    {
        Debug.Log("X: "+x+", Y: "+y);

        if (playerType != _currentPlayerTurn) return; // Verifies if is the correct player in _currentPlayerTurn
        
        OnClickGridPosition?.Invoke(this, new OnClickGridPositionEventArgs
        {
            x = x, 
            y = y, 
            playerType = playerType
        });
        
        UpdateCurrentPlayerTurn();
    }

    private void UpdateCurrentPlayerTurn()
    {
        switch (_currentPlayerTurn) 
        {
            default:
            case PlayerType.Circle:
                _currentPlayerTurn = PlayerType.Triangle;
                break;
            case PlayerType.Triangle:
                _currentPlayerTurn = PlayerType.Circle;
                break;
        }
    }
    public PlayerType GetLocalPlayerType()
    {
        return _localPlayerType;
    }
}
