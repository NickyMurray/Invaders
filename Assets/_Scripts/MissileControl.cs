using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileControl : MonoBehaviour
{
    public Transform spawn;
    public GameObject missileObject;
    public List<GameObject> enemyTargets = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        spawn = GameObject.FindGameObjectWithTag("MissileSpawn").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        for(int i = 0; i < enemyTargets.Count; i++)
        {
            if (enemyTargets[i] == null)
            {
                enemyTargets.RemoveAt(i);
                i--;
            }
        }
	}

    public void SpawnMissiles()
    {
        foreach(GameObject g in enemyTargets)
        {
            GameObject missile = Instantiate(missileObject, spawn.position, spawn.rotation);
            missile.GetComponent<TargetControl>().SetTarget(g.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyTargets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            foreach (GameObject g in enemyTargets)
            {
                if (g == other.gameObject)
                {
                    enemyTargets.Remove(g);
                    break;
                }
            }
        }
    }
}
