using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    [SerializeField] private Transform crateDestroyedPrefab;
    public void Damage()
    {
        Transform crateDestroyTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        ApplyExplosionToCrateParts(crateDestroyTransform, 150f, transform.position, 10f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void ApplyExplosionToCrateParts(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToCrateParts(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
