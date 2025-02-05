using System;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _p1Arrow;
    [SerializeField] private GameObject _p2Arrow;
    [SerializeField] private GameObject _p1YouText;
    [SerializeField] private GameObject _p2YouText;

    private void Awake()
    {
        _p1Arrow.SetActive(false);
        _p2Arrow.SetActive(false);
        _p1YouText.SetActive(false);
        _p2YouText.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnCurrentPlayerTurnChanged += GameManager_OnCurrentPlayerTurnChanged;
    }

    private void GameManager_OnCurrentPlayerTurnChanged(object sender, EventArgs e)
    {
        UpdateCurrentArrow();
    }

    private void GameManager_OnGameStarted(object sender, EventArgs e)
    {
        if(GameManager.Instance.GetLocalPlayerType() == GameManager.PlayerType.Circle) _p1YouText.SetActive(true);
        else _p2YouText.SetActive(true);

        UpdateCurrentArrow();
    }

    private void UpdateCurrentArrow()
    {
        if (GameManager.Instance.GetCurrentPlayerTurn() == GameManager.PlayerType.Circle)
        {
            _p1Arrow.SetActive(true);
            _p2Arrow.SetActive(false);
        }
        else
        {
            _p2Arrow.SetActive(true);
            _p1Arrow.SetActive(false);
        }
    }
}
