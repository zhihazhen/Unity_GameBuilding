using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
    private PlayerManager playerManager;
    
    public Vector3Int lastDetectedPosition = Vector3Int.zero;
    
    [SerializeField]
    private TMP_Text promptText;
    

    private int tempId;
    
    
    
    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
        promptText.text = "";
        

    }

    public void StartPlacement(int ID)
    {
        int money = playerManager.money;
        int resource = playerManager.resource;
        if (money - database.objectsData[ID].Price < 0 && resource - database.objectsData[ID].Resource < 0)
        {
            promptText.text = "You do not have enough money and resource to place.";
            ClearChangedTextAfterDelay();
        }
        else if (resource - database.objectsData[ID].Resource < 0)
        {
            promptText.text = "You do not have enough resource to place.";
            ClearChangedTextAfterDelay();
        }else if(money - database.objectsData[ID].Price < 0)
        {
            promptText.text = "You do not have enough money to place.";
            ClearChangedTextAfterDelay();
        }
        else
        {
            preview.StopShowingPreview();
            gridVisualization.SetActive(true);
            

            tempId = ID;
        
            buildingState = new PlacementState(ID, grid, preview, database, floorData, furnitureData, objectPlacer); 
            playerManager.PreviewMinusMoneyValue(database.objectsData[tempId].Resource, database.objectsData[tempId].Price);
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
        if (buildingState is PlacementState placementState)
        {
            playerManager.MinusMoneyResource(database.objectsData[tempId].Resource,database.objectsData[tempId].Price);
        }
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
    
    private IEnumerator ClearChangedTextAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 延迟一秒
        promptText.text = ""; // 将文本设置为空
    }

}
