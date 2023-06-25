using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;

    private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update() 
    {
        if (!isActive)
        {
            return;
        }

        float stoppingDistance = .1f;
        Vector3 moveDirection = (targetPosition - transform.position).normalized; 

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {              
            float moveSpeed = 4f;     
            transform.position += moveDirection * moveSpeed * Time.deltaTime; 

            unitAnimator.SetBool("IsWalking", true);
        }         
        else
        {
            unitAnimator.SetBool("IsWalking", false);
            ActionComplete();
        }      

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed); 
    }

    public override void TakeAction(GridPosition targetGridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
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

}
