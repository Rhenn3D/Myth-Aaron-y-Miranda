using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{

    private BoxCollider2D boxcollider2D;
    public  GameObject winPanel;
    // Start is called before the first frame update

    void Awake()
    {
        boxcollider2D = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        winPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Win();
        }
    }

    void Win()
    {
        winPanel.SetActive(true);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}