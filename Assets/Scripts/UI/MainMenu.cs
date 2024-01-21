using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState =  CursorLockMode.None;
    }
    
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

    public void Restart ()
    {
        SceneManager.LoadScene("LevelChoice");
    }

    public void Level1 ()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Level2 ()
    {
        SceneManager.LoadScene("Level2");
    }


}
