using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{ 
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObject(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, id, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if(placedObjects.ContainsKey(pos))
                throw new Exception($"Object {pos} already occupied");
            placedObjects[pos] = data;
        }
        
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }

        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return -1;
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition].OccupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> OccupiedPositions;
    public int ID{get;private set;}
    public int PlacedObjectIndex{get;private set;}

    public PlacementData(List<Vector3Int> occupiedPositions, int id, int placedObjectIndex)
    {
        this.OccupiedPositions = occupiedPositions;
        ID = id;
        PlacedObjectIndex = placedObjectIndex;
    }
    
}
