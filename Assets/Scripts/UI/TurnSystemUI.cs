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

    private void Start()
    {
        UpdateTurnText();
        SetEndTurnListener();
        TurnSystem.Instance.OnTurnUpdate += TurnSystem_OnTurnUpdate;
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
    }   

}
