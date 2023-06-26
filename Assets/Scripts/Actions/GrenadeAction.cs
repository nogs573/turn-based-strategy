using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    private const int GRENADE_ACTION_COST = 2;
    [SerializeField] private Transform grenadeProjectilePrefab;
    [SerializeField] private LayerMask obstacleLayerMask;

    private int maxThrowDistance = 7;
    public override string GetActionName()
    {
        return "Grenade";
    }

    public override int GetActionPointsCost()
    {
        return GRENADE_ACTION_COST;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition actionGridPosition)
    {
        return new EnemyAIAction() { gridPosition = actionGridPosition, actionValue = 0 };
    }

    public override List<GridPosition> GetValidActionGridList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //It's outside the bounds of the level grid
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > maxThrowDistance)
                {
                    continue;
                }

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 throwDir = LevelGrid.Instance.GetWorldPosition(testGridPosition) - unitWorldPosition;
                float unitShoulderHeight = 1.7f;

                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, throwDir, Vector3.Distance(unitWorldPosition, LevelGrid.Instance.GetWorldPosition(testGridPosition)), obstacleLayerMask))
                {
                    //Trying to throw grenade through wall
                    continue;
                }

                validGridPositions.Add(testGridPosition);
            }
        }

        return validGridPositions;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);
        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
}
