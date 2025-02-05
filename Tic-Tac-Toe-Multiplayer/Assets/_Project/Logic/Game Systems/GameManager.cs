using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler<OnClickGridPositionEventArgs> OnClickGridPosition;
    public event EventHandler OnGameStarted;
    public event EventHandler OnCurrentPlayerTurnChanged;
        
    private PlayerType _localPlayerType;
    private NetworkVariable<PlayerType> _currentPlayerTurn = new NetworkVariable<PlayerType>();
    
    
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
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        }

        _currentPlayerTurn.OnValueChanged += (PlayerType oldPlayerType, PlayerType newPlayerType) =>
        {
            OnCurrentPlayerTurnChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count == 2)
        {
            // Start the game
            _currentPlayerTurn.Value = PlayerType.Circle;
            TriggerOnGameStartedRpc();
        } 
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TriggerOnGameStartedRpc()
    {
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }
    
    [Rpc(SendTo.Server)]
    public void ClickedOnGridPositionRpc(int x, int y, PlayerType playerType)
    {
        Debug.Log("X: "+x+", Y: "+y);

        if (playerType != _currentPlayerTurn.Value) return; // Verifies if is the correct player in _currentPlayerTurn
        
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
        switch (_currentPlayerTurn.Value) 
        {
            default:
            case PlayerType.Circle:
                _currentPlayerTurn.Value = PlayerType.Triangle;
                break;
            case PlayerType.Triangle:
                _currentPlayerTurn.Value = PlayerType.Circle;
                break;
        }
    }
    
    public PlayerType GetLocalPlayerType()
    {
        return _localPlayerType;
    }

    public PlayerType GetCurrentPlayerTurn()
    {
        return _currentPlayerTurn.Value;
    }
}
