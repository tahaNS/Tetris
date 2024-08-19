using UnityEngine;

[CreateAssetMenu(fileName = "TetrisLevel", menuName = "Tetris/Level")]
public class TetrisLevelData : ScriptableObject
{
    public string[] shapeFilePaths;       
    public float[] shapeChances;          
    public int winningScore;              
    public float speedMin;                
    public float speedStep;               
    public float speedMax;                

}
