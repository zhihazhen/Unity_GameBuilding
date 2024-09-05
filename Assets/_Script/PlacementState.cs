using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
}

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDatabaseSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public PlacementState(int id,
                            Grid grid,
                            PreviewSystem previewSystem,
                            ObjectDatabaseSO database,
                            GridData floorData,
                            GridData furnitureData,
                            ObjectPlacer objectPlacer)
    {
        ID = id;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size);
        }else
            throw new System.Exception($"No object with ID {id}");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlamentValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            return;
        }

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, 
            grid.CellToWorld(gridPosition));
        
        
        
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0?
            floorData : 
            furnitureData;
        selectedData.AddObject(gridPosition, 
            database.objectsData[selectedObjectIndex].Size, 
            database.objectsData[selectedObjectIndex].ID, 
            index);
        
        previewSystem.UpdataPosition(grid.CellToWorld(gridPosition), false);
    }
    
    private bool CheckPlamentValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0?
            floorData : furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        
        bool placementValidity = CheckPlamentValidity(gridPosition, selectedObjectIndex);
        
        previewSystem.UpdataPosition(grid.CellToWorld(gridPosition), placementValidity);
        
        
    }
    
}
