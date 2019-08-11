using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerController : MonoBehaviour {


    public Vector3 movement;
    public float speed = 5;
    Rigidbody rb;
    SphereCollider sCol;
    SphereCollider lCol;
    BoxCollider bCol;
    Animator anim;
	// Use this for initialization
	void Start ()
    {
        sCol = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        bCol = GetComponent<BoxCollider>();
        anim = GetComponentInChildren<Animator>();
	}

    private void Update()
    {
        GetInput();
    }
    // Update is called once per frame
    void FixedUpdate ()
    {       
        Move();
	}

    void GetInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        sCol.enabled = true;
        bCol.enabled = false;
        rb.AddForce(movement * Time.deltaTime * speed);

        if (movement.x == 0 && movement.z == 0)
        {
            sCol.enabled = false;
            bCol.enabled = true;
            anim.SetBool("idle", true);
        }
        else
        {
            anim.SetBool("idle", false);          
        }

        if (movement.z == 0 && movement.x != 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        }
        else if (movement.x == 0 && movement.z != 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }
    }

}
