using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    //Singleton
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayer;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!HandleUnitSelection())
            {
                selectedUnit.Move(MouseWorld.GetPosition());
            }  
        }        
    }

    private bool HandleUnitSelection()
    {
        bool pickedUnit = false;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);   
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayer))
        if (raycastHit.collider != null)
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                pickedUnit = true;
            }
        }

        return pickedUnit;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        //Checks if the event is null (no subscribers), then proceeds with Invoke if not null
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
