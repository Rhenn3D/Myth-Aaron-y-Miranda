using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{


    public GameObject controlsPanel;

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Controls()
    {
       controlsPanel.SetActive(true);
    }

    public void ExitControls()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
