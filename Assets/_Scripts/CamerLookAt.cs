using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerLookAt : MonoBehaviour {


    public GameObject target;
    // Use this for initialization
    void Start ()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(target.transform);
    }
}
