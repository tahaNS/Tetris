using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    // 2D array representing the game board grid, storing the positions of the blocks
    public static Transform[,] gameBorad = new Transform[12, 20];

    // Deletes all full rows on the game board and returns true if any row was deleted
    public static bool DeleteAllFullRows()
    {
        for(int row = 0; row < 20; row++)
        {
            // Check if the current row is full
            if (IsRowFull(row))
            {
                // Delete the full row and play a sound effect
                DeleteGBRow(row);
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rowDelete);
                return true;
            }
        }
        return false;
    }

    // Checks if a specific row on the game board is full
    public static bool IsRowFull(int row)
    {
        for(int col = 0; col < 10; col++)
        {
            if(gameBorad[col , row] == null)
            {
                return false;
            }
        }
        return true;
    }

    // Deletes a specific row and moves down all rows above it
    public static void DeleteGBRow(int row)
    {
        for(int col = 0; col < 10; col++ )
        {
            Destroy(gameBorad[col, row].gameObject);
            gameBorad[col, row] = null;
        }
        row++;

        for(int j = row; j < 20; j++)
        {
            for(int col = 0; col < 10; col++)
            {
                if(gameBorad[col,j] != null)
                {
                    gameBorad[col, j - 1] = gameBorad[col, j];
                    gameBorad[col, j] = null;
                    gameBorad[col, j - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }
}
