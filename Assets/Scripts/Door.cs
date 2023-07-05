using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen;
    private Animator doorAnimator;
    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;

    private void Awake()
    {
        doorAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);        
       
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
        }
    }
    public void Interact(Action onInteractComplete)
    {
        this.onInteractionComplete = onInteractComplete;
        isActive = true;
        timer = 0.5f;

        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        doorAnimator.SetBool("IsOpen", isOpen);
    }

    private void OpenDoor()
    {
        isOpen = true;
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void CloseDoor()
    {
        isOpen = false;
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
