using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public GameObject explosion;
    Transform target;
    Vector3 distance;
    Vector3 direction;
    Vector3 targetPos;
    public float speed = 50f;
    public float damage = 5f;
    public bool hit = false;
    public bool tracker = false;
    public float timeBetweenTargetChecks = 0.5f;
    float counter;
	// Use this for initialization
	void Start ()
    {
        Physics.IgnoreLayerCollision(10, 10);
        if (!tracker) Destroy(gameObject, 5f);
        else Destroy(gameObject, 10f);
        counter = timeBetweenTargetChecks;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!hit)
        {
            if (tracker)Track();
            else Move();
        }
            
	}

    void Move()
    {
        distance = targetPos - transform.position;
        direction = distance.normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void Track()
    {
        counter += Time.deltaTime; 
        if (counter >= timeBetweenTargetChecks)
        {
            counter = 0;
            distance = target.position - transform.position;
            direction = distance.normalized;
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        targetPos = target.position;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player" && !hit)
        {
            col.gameObject.GetComponent<TankController>().TakeDamage(damage);
        }
        hit = true;
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
