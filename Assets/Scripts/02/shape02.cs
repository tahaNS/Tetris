using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class shape02 : MonoBehaviour
{

    public TetrisLevelData levelData;
    public static float speed  ;
    public static float speedMax;
    public static float speedStep;
    float lastMoveDown = 0;
    public static int Winscore;
    public Transform pivot;  
    int score ;
   
    void Start()
    {

        // Check if the shape is within the grid at the start

        if (!IsInGrid())
        {
            Debug.Log("Game Over scene is about to load...");
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.gameOver);
            Invoke("OpenGameOverScene", 0.1f);
        }

        // Initialize speed and win score from level data
        speed = levelData.speedMin;
        speedMax = levelData.speedMax;
        speedStep = levelData.speedStep;
        Winscore = levelData.winningScore;
    }
    // Opens the Game Over scene
    void OpenGameOverScene()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }

    // Increases the speed of the shape's movement
    void IncreaseSpeed()
    {
        
      
        if(shape02.speed>speedMax)
        {

            shape02.speed -= speedStep;
        }

    }
    // Called every frame to handle user input and shape movement
    void Update()
    {
        
        if (Input.GetKeyDown("a"))
        {
            transform.position += new Vector3(-1, 0, 0);

            Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.position += new Vector3(1, 0, 0);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }

        }
        if (Input.GetKeyDown("d"))
        {
            transform.position += new Vector3(1, 0, 0);

            Debug.Log(transform.position);
            if (!IsInGrid())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }

        }
        if (Input.GetKeyDown("s") || Time.time - lastMoveDown >= shape02.speed)
        {
            
            Debug.Log("speed: "+ shape02.speed);
            transform.position += new Vector3(0, -1, 0);
            
            Debug.Log(transform.position);
            if (!IsInGrid())
            {
                transform.position += new Vector3(0, 1, 0);

                bool rowDeleted = GameBoard.DeleteAllFullRows();
                if (rowDeleted)
                {
                    GameBoard.DeleteAllFullRows();
                    //Score
                    IncreaseTextUIScore();
                }
                Debug.Log("IS in grid N work");
                enabled = false;
                IncreaseSpeed();
                Debug.Log("spawn shape");
                FindObjectOfType<ShapeSpowner01>().SpawnShape();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeStop);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }


            lastMoveDown = Time.time;
        }
        if (Input.GetKeyDown("w"))
        {
            RotateAroundPivot();


        }
    }
    // Checks if the shape is within the grid boundaries
    public bool IsInGrid()
    {
        
        foreach (Transform childBlock in transform)
        {
            Vector2 vect = RoundVector(childBlock.position);
           

            if (!IsInBorder(vect))
            {
                return false;
            }
            if (GameBoard.gameBorad[(int)vect.x, (int)vect.y] != null && GameBoard.gameBorad[(int)vect.x, (int)vect.y].parent != transform)
            {
                return false;
            }

        }
        return true;
    }

    // Rounds a Vector2 to the nearest whole number
    public Vector2 RoundVector(Vector2 vect)
    {
        return new Vector2(Mathf.Round(vect.x), Mathf.Round(vect.y));
    }

    // Checks if the position is within the grid's borders
    public static bool IsInBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x <= 9 && (int)pos.y >= 0);
    }

    // Updates the game board based on the current shape's position
    public void UpdateGameBoard()
    {
        for (int y = 0; y < 20; ++y)
        {
            for (int x = 0; x < 10; ++x)
            {
                if (GameBoard.gameBorad[x, y] != null && GameBoard.gameBorad[x, y].parent == transform)
                {
                    GameBoard.gameBorad[x, y] = null;
                }
            }
        }

        foreach (Transform childBlock in transform)
        {
            Vector2 vect = RoundVector(childBlock.position);
            if (vect.y < 20)
            {
                GameBoard.gameBorad[(int)vect.x, (int)vect.y] = childBlock;

                Debug.Log("Cube At :" + vect.x + " " + vect.y);
            }

           
        }

    }

    // Increases the score displayed on the UI
    void IncreaseTextUIScore()
    {
        var textUIComp = GameObject.Find("ScoreNUM").GetComponent<Text>();
        score = int.Parse(textUIComp.text);
        score++;
        textUIComp.text = score.ToString();
        WinFun(score);

    }

    // Rotates the shape around its pivot point
    void RotateAroundPivot()
    {
        if (pivot == null)
        {
            Debug.LogError("Cannot rotate: Pivot is null.");
            return;
        }

        // Store original positions
        List<Vector3> originalPositions = new List<Vector3>();
        foreach (Transform childBlock in transform)
        {
            originalPositions.Add(childBlock.position);
        }

        // Rotate the shape around the pivot
        foreach (Transform childBlock in transform)
        {
            Vector3 direction = childBlock.position - pivot.position; 
            direction = Quaternion.Euler(0, 0, 90) * direction; 
            childBlock.position = pivot.position + direction; 
        }

       
        if (!IsInGrid())
        {
            
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position = originalPositions[i];
            }
            Debug.Log("Rotation invalid, reverting to original positions.");
        }
        else
        {
            UpdateGameBoard();
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.rotateSound);
        }
    }

    // Checks if the player has won the level
    void WinFun(int score01)
    {
        Text Level = GameObject.Find("Level").GetComponent<Text>();
        string textContent = Level.text;
        if (score01 >= Winscore)
        {
            Debug.Log("Winscore=   " + Winscore);
            Debug.Log("score=   " + score01);
            if(textContent == "LEVEL 01")
            {
                SceneManager.LoadScene("Win 1");
            }
            if (textContent == "LEVEL 02")
            {
                SceneManager.LoadScene("Win 2");
            }
            if (textContent == "LEVEL 03")
            {
                SceneManager.LoadScene("Win 3");
            }

        }
    }
}
