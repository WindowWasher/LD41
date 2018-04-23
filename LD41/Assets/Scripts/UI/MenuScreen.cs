using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour {

    public GameObject menuPanel;
    public Text menuText;
    public Button restartButton;
    public Button exitButton;

	// Use this for initialization
	void Start () {
        menuPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
	}

    public void OnRestartButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnButtonExit()
    {
        Application.Quit();
    }
}
