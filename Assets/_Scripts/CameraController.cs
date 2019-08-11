using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;
	void Start ()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate ()
    {    
        transform.LookAt(target.transform); 
	}
}
