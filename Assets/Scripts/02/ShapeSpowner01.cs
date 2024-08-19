using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpowner01 : MonoBehaviour
{
    public TetrisLevelData levelData;
    public ShapeLoader shapeLoader;         
    public GameObject[] nextShapes;         
    
    private Dictionary<int, GameObject> shapePrefabs = new Dictionary<int, GameObject>();  
    private GameObject upNextObject = null;
    private int shapeIndex = 0;             
    private int nextShapeIndex = 0;         

    void Start()
    {
        // Validate shape data
        if (levelData.shapeFilePaths == null || levelData.shapeChances == null || levelData.shapeFilePaths.Length != levelData.shapeChances.Length)
        {
            Debug.LogError("Shape file paths or chances are not properly initialized.");
            return;
        }

        SetNextShapeIndex();
        Debug.Log("Starting to Spawn Shape.");
        SpawnShape();
    }

    // Spawns the current shape and sets the next shape
    public void SpawnShape()
    {
       
        shapeIndex = nextShapeIndex;
        GameObject shapePrefab = GetShapePrefab(shapeIndex);
        if (shapePrefab != null)
        {
            Vector3 spawnPosition = transform.position;  
            // Instantiate the new shape
            Instantiate(shapePrefab, spawnPosition, Quaternion.identity);
            Destroy(shapePrefab);
        }
        else
        {
            Debug.LogError("Shape prefab is null.");
        }

        SetNextShapeIndex();
        Vector3 nextShapePos = new Vector3(-7f, 16f, 0);

        // Destroy the previous "up next" shape if it exists
        if (upNextObject != null)
        {
            Debug.Log("Destroying previous 'up next' shape.");
            Destroy(upNextObject);
        }

        // Instantiate the new "up next" shape
        if (nextShapeIndex < nextShapes.Length && nextShapes[nextShapeIndex] != null)
        {
            Debug.Log($"Instantiating 'up next' shape at Position: {nextShapePos}");
            upNextObject = Instantiate(nextShapes[nextShapeIndex], nextShapePos, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Next shape prefab is null or index is out of bounds.");
        }
    }

    // Sets the index of the next shape to spawn
    private void SetNextShapeIndex()
    {
        int previousIndex = nextShapeIndex;
        nextShapeIndex = GetRandomShapeIndexBasedOnChance();


        Debug.Log($"Next Shape Index set to: {nextShapeIndex}");
    }

    // Retrieves the shape prefab based on the given index
    private GameObject GetShapePrefab(int index)
    {

        if (index >= 0 && index < levelData.shapeFilePaths.Length)
        {
            string filePath = levelData.shapeFilePaths[index];
            GameObject prefab = shapeLoader.LoadShapeFromText(filePath);
            shapePrefabs[index] = prefab;
        }
        else
        {
            Debug.LogError("Index is out of bounds.");
            return null;
        }
        return shapePrefabs[index];
    }

    // Gets a random shape index based on the chances provided
    private int GetRandomShapeIndexBasedOnChance()
    {
        float totalChance = 0f;
        foreach (float chance in levelData.shapeChances)
        {
            totalChance += chance;
        }

        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        for (int i = 0; i < levelData.shapeChances.Length; i++)
        {
            cumulativeChance += levelData.shapeChances[i];
            if (randomValue < cumulativeChance)
            {
                return i;
            }
        }

        return levelData.shapeChances.Length - 1;  
    }
    
}
