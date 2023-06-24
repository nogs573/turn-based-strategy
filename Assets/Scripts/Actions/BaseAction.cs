using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    //a default delegate type
    protected Action onActionComplete;
    
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    //Forced to implement in all child classes
    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidActionGridList();
        return validGridPositions.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridList();
}
