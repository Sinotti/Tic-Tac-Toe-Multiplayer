using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public EventHandler<OnClickGridPositionEventArgs> OnClickGridPosition;

    public class OnClickGridPositionEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    private void Awake()
    {
        if(Instance != null) Destroy(this);
        Instance = this;
    }

    public void ClickedOnGridPosition(int x, int y)
    {
        Debug.Log("X: "+x+", Y: "+y);
        OnClickGridPosition?.Invoke(this, new OnClickGridPositionEventArgs { x = x, y = y });
    }
}
