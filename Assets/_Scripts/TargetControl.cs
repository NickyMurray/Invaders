using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControl : MonoBehaviour {

    public GameObject explosion;
    Transform target;
    Vector3 distance;
    Vector3 direction;
    public float speed = 50f;
    public float damage = 200f;
    public bool hit = false;

    private void Start()
    {
        Destroy(gameObject, 15f);
    }
    // Update is called once per frame
    void Update ()
    {
        if (!hit)
        {
            if(target != null)Track();
        }
    }

    void Track()
    {
        distance = target.position - transform.position;
        direction = distance.normalized;

        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(target);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (!hit)
            {
                col.gameObject.GetComponent<EnemyController>().TakeDamage(damage, false);
            }
            hit = true;
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
