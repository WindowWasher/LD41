using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuScreen : MonoBehaviour {

    public GameObject menuPanel;
    public Text menuText;
    public Button restartButton;
    public Button exitButton;
    public Timer timer;

	// Use this for initialization
	void Start () {
        timer = new Timer();
        menuPanel.SetActive(false);
        timer.Start(3f);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
        
        Debug.Log(BuildingInfoManager.instance.getAllActiveBuildings().Where(b => !b.GetComponent<Building>().IsWall()).Count());
        if (timer.Expired() && BuildingInfoManager.instance.getAllActiveBuildings().Where(b => !b.GetComponent<Building>().IsWall()).Count() <= 0)
        {
            menuPanel.SetActive(true);
            menuText.text = "You Loose";
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
