using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float spunAmount;
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        
        spunAmount += spinAddAmount;
        if (spunAmount >= 360)
        {
            isActive = false;
            onActionComplete();
        }        
    }
    public void Spin(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        spunAmount = 0f;
        isActive = true;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
