using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour {

    public Slider health;
    public Slider shield;
    public Text txtScore;
    public Text txtDropPods;
    public Text txtAchievements;
    public GameObject txtMissileText;
    public GameObject player;
    public GameObject deathScreen;
    public GameObject pauseScreen;
    public Text txtDeathScore;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        deathScreen.SetActive(false);
        pauseScreen.SetActive(false);
        txtMissileText.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        health.value = player.GetComponent<TankController>().health;
        shield.value = player.GetComponent<TankController>().shield;
        txtScore.text = "SCORE: " + GameSettings.instance.curScore;
        txtDropPods.text = "DROPPODS LEFT: " + GameSettings.instance.dropPods;
	}

    public void DropPodCleared()
    {
        txtAchievements.text = "DROP POD CLEARED!!";
        txtAchievements.GetComponent<Animator>().SetTrigger("dropPod");

    }

    public void ShieldsDown()
    {
        txtAchievements.text = "SHIELDS ARE DOWN!!!!";
        txtAchievements.GetComponent<Animator>().SetTrigger("dropPod");
    }

    public void ShowDeathScreen()
    {
        Time.timeScale = 0;
        if(!deathScreen.activeInHierarchy)deathScreen.SetActive(true);
        txtDeathScore.text = "DROPPODS LEFT: " + GameSettings.instance.dropPods + "\nSCORE: " + GameSettings.instance.curScore;
    }

    public void ShowPauseScreen()
    {
        Time.timeScale = 0;
        if(!pauseScreen.activeInHierarchy)pauseScreen.SetActive(true);
    }

    public void HidePauseScreen()
    {
        Time.timeScale = 1;
        player.GetComponent<TankController>().state = 0;
        pauseScreen.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameSettings.instance.curScore = 0;
        GameSettings.instance.dropPods = 0;
        Time.timeScale = 1;
        player.GetComponent<TankController>().state = 0;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        
    }

    public void SetMissileTextActive(bool enable)
    {
        txtMissileText.SetActive(enable);
    }
}
