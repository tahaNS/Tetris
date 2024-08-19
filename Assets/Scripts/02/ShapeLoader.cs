using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShapeLoader : MonoBehaviour
{
    // Public prefabs for different shapes
    public GameObject squarePrefab;
    public GameObject lShapePrefab;
    public GameObject iShapePrefab;
    public GameObject jShapePrefab;
    public GameObject sShapePrefab;
    public GameObject tShapePrefab;
    public GameObject zShapePrefab;

    // Loads a shape from a text file and returns the corresponding GameObject
    public GameObject LoadShapeFromText(string filePath)
    {
        
        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length < 6)
        {
            Debug.LogError("Shape file is not in the expected format.");
            return null;
        }

        string shapeIdentifier = lines[0];  // The first line is the shape identifier
        string[] shapeGrid = new string[lines.Length - 1];
        System.Array.Copy(lines, 1, shapeGrid, 0, lines.Length - 1);

        return CreateShapePrefab(shapeGrid, shapeIdentifier);
    }

    // Creates a shape prefab based on the grid data and shape name
    private GameObject CreateShapePrefab(string[] grid, string shapeName)
    {
        // Determine which prefab to use based on the shape name
        GameObject selectedPrefab = null;

        switch (shapeName)
        {
            case "O":
                selectedPrefab = squarePrefab;
                break;
            case "L":
                selectedPrefab = lShapePrefab;
                break;
            case "I":
                selectedPrefab = iShapePrefab;
                break;
            case "J":
                selectedPrefab = jShapePrefab;
                break;
            case "S":
                selectedPrefab = sShapePrefab;
                break;
            case "Z":
                selectedPrefab = zShapePrefab;
                break;
            case "T":
                selectedPrefab = tShapePrefab;
                break;
            default:
                Debug.LogError($"No prefab found for shape: {shapeName}");
                return null;
        }

        // Create a new GameObject for the shape prefab
        GameObject shapeInstance = new GameObject(shapeName);

        // Add the Shape script to the shape instance
        shape02 shapeScript = shapeInstance.AddComponent<shape02>();

        // Variable to store the pivot block
        Transform pivot = null;

        // Create blocks based on the grid data
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                char cell = grid[row][col];
                if (cell == 'X' || cell == 'O')
                {
                    GameObject block = CreateBlock(shapeInstance.transform, row, col, cell, selectedPrefab);

                    // If this block is the pivot, store it
                    if (cell == 'O')
                    {
                        pivot = block.transform;
                    }
                }
            }
        }

        
        shapeScript.pivot = pivot;

        return shapeInstance;
    }

    // Creates a block at the specified row and column using the provided prefab
    private GameObject CreateBlock(Transform parent, int row, int col, char cell, GameObject prefab)
    {
        GameObject block = Instantiate(prefab);
        block.transform.SetParent(parent);

        // Position the block based on its row and column in the grid
        block.transform.localPosition = new Vector3(col, -row, 0);

        // Return the block so it can be assigned as a pivot if needed
        return block;
    }
}
