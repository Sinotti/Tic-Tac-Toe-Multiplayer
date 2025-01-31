using System;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    [SerializeField] private int _coordinateX;
    [SerializeField] private int _coordinateY;
    private void OnMouseDown()
    {
        GameManager.Instance.ClickedOnGridPosition(_coordinateX, _coordinateY);
    }
}
