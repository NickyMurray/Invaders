using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public static GameSettings instance;
    public int curScore = 0;
    public int dropPods = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    public void SetScore(int amount)
    {
        curScore += amount;
    }

    public void AddDropPod()
    {
        dropPods++;
    }

    public void RemoveDropPod()
    {
        dropPods--;
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvas.GetComponent<CanvasController>().DropPodCleared();
    }
}
