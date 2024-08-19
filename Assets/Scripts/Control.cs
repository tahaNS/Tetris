using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Control : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Get the name of the scene
        string sceneName = currentScene.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetTheGame()
    {
        SceneManager.LoadScene("SampleScene");

    }
    public void Level02()
    {
        
        SceneManager.LoadScene("Level02");

    }
    public void Level03()
    {

        SceneManager.LoadScene("Level03");

    }
}
