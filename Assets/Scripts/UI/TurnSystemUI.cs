using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnCounterText;
    [SerializeField] private GameObject enemyTurnVisualGameObject;

    private void Start()
    {
        
        TurnSystem.Instance.OnTurnUpdate += TurnSystem_OnTurnUpdate;

        UpdateTurnText();
        SetEndTurnListener();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    public void UpdateTurnText()
    {
        turnCounterText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    public void SetEndTurnListener()
    {
        endTurnButton.onClick.AddListener(() => {
            TurnSystem.Instance.NextTurn();
        });                
    }

    private void TurnSystem_OnTurnUpdate(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }   

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }


}
