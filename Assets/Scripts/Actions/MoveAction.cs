using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private int maxMoveDistance = 4;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    private void Update() 
    {
        if (!isActive)
        {
            return;
        }

        Vector3 targetposition = positionList[currentPositionIndex];        
        Vector3 moveDirection = (targetposition - transform.position).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetposition) > stoppingDistance)
        {              
            float moveSpeed = 4f;     
            transform.position += moveDirection * moveSpeed * Time.deltaTime;            
        }         
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }       
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> gridPath = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in gridPath)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }    

    public override List<GridPosition> GetValidActionGridList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //It's outside the bounds of the level grid
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    //Same position where the unit already is
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //Grid position already occupied by other
                    continue;
                }

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    //Grid position is not walkable
                    continue;
                }

                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    //Unreachable grid position
                    continue;
                }

                int pathfindingDistanceMultiplier = 10; //multiplier we used when calculating G, H, FCost
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
                {
                    //Path length is bigger than max move distance
                    continue;
                }

                validGridPositions.Add(testGridPosition);
                //Debug.Log(testGridPosition);
            }
        }

        return validGridPositions;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }

}
