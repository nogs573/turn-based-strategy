using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{       
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints = 2;

    private void Awake() 
    {
        moveAction = GetComponent<MoveAction>();      
        spinAction = GetComponent<SpinAction>();  
        baseActionArray = GetComponents<BaseAction>();
    }
    
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        //Go into level grid and update current grid position
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }
    

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPoints(BaseAction baseAction)
    {
        if (CanSpendActionPoints(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPoints(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPointsCost();
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }
}
