using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuScreen : MonoBehaviour {

    public GameObject menuPanel;
    public Text menuText;
    public Button resumeButton;
    public Button restartButton;
    public Button exitButton;
    public Timer timer;

    Button beginButton;
    GameObject instructions;

    // Use this for initialization
    void Start () {

        beginButton = GameObject.Find("BeginButton").GetComponent<Button>();
        beginButton.onClick.AddListener(BeginLevel);

        instructions = GameObject.Find("Instructions");

        resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        resumeButton.onClick.AddListener(ResumeClick);

        resumeButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        Time.timeScale = 0;

        timer = new Timer();
        
        timer.Start(3f);
    }


    void BeginLevel()
    {
        Time.timeScale = 1;
        beginButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        menuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            if (menuPanel.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        
        if (timer.Expired() && BuildingInfoManager.instance.getAllActiveBuildings().Where(b => !b.GetComponent<Building>().IsWall()).Count() <= 0)
        {
            menuPanel.SetActive(true);
            instructions.GetComponentInChildren<Text>().text = "The barbarians have burned your city to the ground! There were no surviors.";
        }
    }

    public void ResumeClick()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
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
