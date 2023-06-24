using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
	public static GridSystemVisual Instance { get; private set; }
	[SerializeField] private Transform gridSystemVisualSinglePrefab;

	private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

	private int width;
	private int height;

	private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GridSystemVisual! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
	{
		width = LevelGrid.Instance.GetWidth();
		height = LevelGrid.Instance.GetHeight();

		gridSystemVisualSingleArray = new GridSystemVisualSingle[width,height];
		
		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				GridPosition gridPosition = new GridPosition(x, z);
				Transform gridSystemVisualSingleTransform =  Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

				gridSystemVisualSingleArray[x,z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
			}
		}
	}

	private void Update()
	{
		UpdateGridVisual();
	}

	public void HideAllGridPositions()
	{
		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				gridSystemVisualSingleArray[x,z].Hide();
			}
		}

	}

	public void ShowGridPositionList(List<GridPosition> gridPositionList)
	{
		foreach (GridPosition gridPosition in gridPositionList)
		{
			gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
		}
	}

	private void UpdateGridVisual()
	{
		HideAllGridPositions();

		Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

		ShowGridPositionList(selectedUnit.GetMoveAction().GetValidActionGridList());
	}
}
