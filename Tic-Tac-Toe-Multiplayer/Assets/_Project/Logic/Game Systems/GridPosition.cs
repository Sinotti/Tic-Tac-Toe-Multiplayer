using System;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    [SerializeField] private int _coordinateX;
    [SerializeField] private int _coordinateY;
    private void OnMouseDown()
    {
        GameManager.Instance.ClickedOnGridPositionRpc(_coordinateX, _coordinateY, GameManager.Instance.GetLocalPlayerType());
    }
}
