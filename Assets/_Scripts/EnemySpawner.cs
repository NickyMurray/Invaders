using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    // Waypoint Objects
    public List<Transform> waypoints = new List<Transform>();

    // Objects to Spawn
    public List<GameObject> spawns = new List<GameObject>();

    //Objects that were spawned
    public List<GameObject> spawnedObjects = new List<GameObject>();

    //Amount of Objects to spawn
    public int spawnAmount = 3;

    //Droppod cleared of enemies
    bool cleared = false;
	// Use this for initialization
	void Start ()
    {
        int wayPointIndex = 0;
        for (int i = 0; i < spawnAmount; i++)
        {
            int randIndex = Random.Range(0, spawns.Count);
            GameObject spawn = Instantiate(spawns[randIndex], waypoints[wayPointIndex].position, Quaternion.Euler(Vector3.zero));
            spawn.name = spawns[randIndex].name;
            spawn.GetComponent<EnemyController>().SetWaypoints(waypoints);
            spawnedObjects.Add(spawn);
            wayPointIndex++;
            if (wayPointIndex >= waypoints.Count)
            {
                wayPointIndex = 0;
            }
        }
        GameSettings.instance.AddDropPod();
	}

    void Update()
    {
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                i--;
            }
        }

        if (spawnedObjects.Count == 0 && cleared == false)
        {
            GameSettings.instance.RemoveDropPod();
            cleared = true;
        }
    }
}


