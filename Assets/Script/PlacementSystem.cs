using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] 
    private ObjectDatabaseSO database;

    [SerializeField] private GameObject gridVisualization;

    private GridData floorData, furnitureData;
    
    [SerializeField]
    private ObjectPlacer objectPlacer;

    [SerializeField] 
    private PreviewSystem preview;

    [SerializeField] 
    private MoneyManager moneyManager;
    public Vector3Int lastDetectedPosition = Vector3Int.zero;

    private int temp;
    
    
    
    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
        

    }

    public void StartPlacement(int ID)
    {
        Debug.Log($"{ID}");
        int money = moneyManager.money;
        if (money - this.database.objectsData[ID].Price < 0)
        {
            Debug.Log("not enough money");
        }
        else
        {
            preview.StopShowingPreview();
            gridVisualization.SetActive(true);

            temp = ID;
        
            buildingState = new PlacementState(ID, grid, preview, database, floorData, furnitureData, objectPlacer); 
            /*
            inputManager.OnClicked += PlaceStructure;
            moneyManager.MinusMoney(database.objectsData[ID].Price);
            
            inputManager.OnExit += StopPlacement;
            */
            // 订阅 OnClicked 事件
            inputManager.OnClicked += OnFirstClick;
            // 订阅 OnExit 事件
            inputManager.OnExit += StopPlacement;
        }
        
    }

    private void OnFirstClick()
    {
        PlaceStructure();
        
        if (buildingState is PlacementState placementState)
        {
            moneyManager.MinusMoney(database.objectsData[temp].Price);
        }
        
        StopPlacement();
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, database, floorData, furnitureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        
        buildingState.OnAction(gridPosition);
    }

    
    private void StopPlacement()
    {
        if (buildingState == null)
        {
            return;
        }
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }

    }

}
