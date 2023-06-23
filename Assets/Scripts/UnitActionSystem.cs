using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayer;

    private void Update()
    {
        if (!HandleUnitSelection())
        {
            if (Input.GetMouseButton(0))
            {
                selectedUnit.Move(MouseWorld.GetPosition());
            }  
        }        
    }

    private bool HandleUnitSelection()
    {
        bool pickedUnit = false;
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);   
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayer))
            if (raycastHit.collider != null)
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    selectedUnit = unit;
                    pickedUnit = true;
                }
            }
        }

        return pickedUnit;
    }
}
