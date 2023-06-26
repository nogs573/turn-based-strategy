using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemiesRemainingText;
    [SerializeField] private GameObject gameWinObject;
    [SerializeField] private GameObject gameLoseObject;
    [SerializeField] private GameObject instructionsObject;
    [SerializeField] private GameObject endMessageText;

    private void Awake()
    {
        ClearEndScreen();
    }

    private void Start()
    {
        UnitManager.Instance.OnEnemyListChanged += UnitManager_OnEnemyListChanged;
        UnitManager.Instance.OnAllFriendlyUnitDead += UnitManager_OnAllFriendlyUnitDead;
        UpdateEnemiesRemaining(UnitManager.Instance.GetEnemyCount());
    }

    private void UnitManager_OnAllFriendlyUnitDead(object sender, System.EventArgs e)
    {
        ShowEndScreen(false);
    }

    private void UnitManager_OnEnemyListChanged(object sender, System.EventArgs e)
    {
        int enemiesLeft = UnitManager.Instance.GetEnemyCount();
        UpdateEnemiesRemaining(enemiesLeft);
        if (enemiesLeft == 0)
        {
            ShowEndScreen(true);
        }
    }

    public void UpdateEnemiesRemaining(int enemiesLeft)
    {
        enemiesRemainingText.text = "Enemies remaining: " + UnitManager.Instance.GetEnemyCount().ToString();
    }

    public void ShowEndScreen(bool HasWon)
    {
        if (HasWon)
        {
            gameWinObject.SetActive(true);
        }
        else
        {
            gameLoseObject.SetActive(true);
        }

        endMessageText.SetActive(true);
        instructionsObject.SetActive(true);
    }

    public void ClearEndScreen()
    {
        gameWinObject.SetActive(false);
        gameLoseObject.SetActive(false);
        endMessageText.SetActive(false);
        instructionsObject.SetActive(false);
    }


}
