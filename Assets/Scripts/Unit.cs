using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    float moveSpeed = 4f;
    float stoppingDistance = .1f;

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;        
            transform.position += moveDirection * moveSpeed * Time.deltaTime;  
        }         

        if (Input.GetMouseButton(0))
        {
            Move(MouseWorld.GetPosition());
        }     
    }

    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }    
}
