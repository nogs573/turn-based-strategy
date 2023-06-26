using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    public void Damage()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }
}
