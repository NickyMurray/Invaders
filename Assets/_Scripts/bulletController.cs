using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour {

    public GameObject explosion;
    public float speed = 50f;
    public float damage = 10f;
    public bool hit = false;
    // Use this for initialization
	void Start ()
    {
        Physics.IgnoreLayerCollision(9, 9);
        Destroy(gameObject, 5f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!hit)
        {
            transform.Translate(new Vector3(0,-0.1f,1f) * speed * Time.deltaTime);
        }
	}

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" && !hit)
        {
            col.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            hit = true;
        }

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
