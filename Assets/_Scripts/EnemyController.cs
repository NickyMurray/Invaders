using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public enum EnemyState { GUARD, CHASE, EVADE, ATTACK, DEAD }

    public EnemyState state = EnemyState.GUARD;

    //Enemy Health Variables
    public bool damaged = false;
    public float maxHealth = 40f;
    public float health = 40f;
    public GameObject explosion;
    public int scroreValue = 10;

    //Movement Variables / Objects
    public Transform target;
    public Vector3 direction;
    Vector3 distance;
    Vector3 oldTargetPos;
    Vector3 newTargetPos;
    Vector3 oldPosition;
    Vector3 relativeVelocity;
    public float lookAheadDistance;
    public float speed = 500f;

    //Attack Variables / Objects
    public bool attacking = false;
    public GameObject laser;
    public bool shooter = false;
    public float shootRange = 5;
    public float distanceToTarget;
    float counter = 0f;
    public float timeBetweenShots = 2f;
    public GameObject bullet;
    public GameObject muzzle;

    //Enemy Object Parts
    public List<GameObject> bodyParts = new List<GameObject>();
    Animator anim;
    public List<Transform> waypoints;
    public int curWaypoint = 0;
	// Use this for initialization
	void Start ()
    {
        counter = timeBetweenShots;
        anim = GetComponent<Animator>();
        if(laser != null)laser.SetActive(false);
	}

    // Update is called once per frame
    void Update ()
    {
        if (state != EnemyState.DEAD && !damaged && !attacking)
        {
            switch (state)
            {
                case EnemyState.GUARD:
                    MoveToWaypoints();
                    break;
                case EnemyState.CHASE:
                    ChaseTarget();
                    CheckDistance();
                    break;
                case EnemyState.EVADE:
                    EvadeTarget();
                    break;
                case EnemyState.ATTACK:
                    Attack();
                    break;
            }
        }
        CheckAttackState();
        Animate();
        
    }

    void Animate()
    {
        anim.SetFloat("zAxis", Mathf.Abs(distance.z));
    }

    



    //GUARD METHODS
    void MoveToWaypoints()
    {
        if (waypoints != null && waypoints.Count > 0 && !attacking)
        {
            WaypointDirection();
            transform.position += direction * speed/2 * Time.deltaTime;
            transform.forward = direction;         
        }
    }

    void WaypointDirection()
    {
        distance = waypoints[curWaypoint].position - transform.position;
        direction = distance.normalized;
    }

    public void SetWaypoints(List<Transform> newWaypoints)
    {
        waypoints = newWaypoints;
    }

    //CHASE METHODS
    void ChaseTarget()
    {        
            GetTargetVelocity();
            if (distance.magnitude >= speed * Time.deltaTime)
            {
                transform.position += direction * speed * Time.deltaTime;
                transform.forward = direction;
            }
            else state = EnemyState.ATTACK;
    }

    void GetTargetVelocity()
    {
        newTargetPos = target.position;

        //targets velocity
        Vector3 deltaTargetPos = newTargetPos - oldTargetPos;

        //this objects velocity
        Vector3 deltaPos = transform.position - oldPosition;

        //closing velocity
        relativeVelocity = deltaTargetPos - deltaPos;

        //Distance to close
        distance = newTargetPos - transform.position;


        //T = |distance| / |relativeVelocity|
        lookAheadDistance = distance.magnitude / (Mathf.Approximately(relativeVelocity.magnitude, Mathf.Epsilon) ? 0.01f : relativeVelocity.magnitude);

        Vector3 playerTargetPos = target.position + deltaTargetPos * lookAheadDistance;

        distance = playerTargetPos - transform.position;

        direction = distance.normalized;

        oldTargetPos = newTargetPos;
        oldPosition = transform.position;

        distanceToTarget = distance.magnitude;
    }

    //EVADE METHODS
    void EvadeTarget()
    {
        GetTargetVelocity();
        transform.position += direction * -speed * Time.deltaTime;
        transform.forward = -direction;
        HealOverTime();
    }

    void HealOverTime()
    {
        if (health < maxHealth / 2)
        {
            health += 1 * Time.deltaTime;
        }
        else
        {
            state = EnemyState.CHASE;
        }
    }

    //ATTACK METHODS
    void Attack()
    {
        transform.forward = target.position - transform.position;
        GetTargetVelocity();
        
        anim.SetTrigger("Attack");
        if (shooter)
        {
            Shoot();           
        }

        if (distance.magnitude >= shootRange)
        {
            state = EnemyState.CHASE;
        }

        
    }

    void CheckDistance()
    {
        if (distance.magnitude <= shootRange && counter >= timeBetweenShots)
        {
            state = EnemyState.ATTACK;
        }
    }


    void CheckAttackState()
    {
        if (attacking && !shooter)
        {
            laser.SetActive(true);
        }
        else if (!attacking && !shooter)
        {
            laser.SetActive(false);
        }

        if (counter < timeBetweenShots)
        {
            counter += Time.deltaTime;
        }
    }
    void Shoot()
    {
        if (counter >= timeBetweenShots)
        {
            counter = 0f;
            GameObject shot = Instantiate(bullet, muzzle.transform.position, Quaternion.Euler(Vector3.zero));
            shot.name = bullet.name;
            shot.GetComponent<EnemyBulletController>().SetTarget(target);
        }
    }

    //HEALTH MEHODS
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            state = EnemyState.DEAD;
            Die();
        }
        else if(health <= maxHealth/4)
        {
            state = EnemyState.EVADE;
        }
    }
    public void TakeDamage(float amount, bool countScore)
    {
        health -= amount;
        if (health <= 0)
        {
            state = EnemyState.DEAD;
            scroreValue = 0;
            Die();
        }
        else if (health <= maxHealth / 4)
        {
            state = EnemyState.EVADE;
        }
    }


    void Die()
    {
        anim.SetBool("Dead", true);
        GameSettings.instance.SetScore(scroreValue);
        FallApart();
        Destroy(gameObject, 5f);
        Instantiate(explosion, transform.position, transform.rotation);
    }

    void FallApart()
    {
        if (bodyParts.Count != 0)
        { 
            foreach (GameObject g in bodyParts)
            {
                if (g.GetComponent<Rigidbody>() == null)
                {
                    g.AddComponent<Rigidbody>();
                }
                else
                {
                    g.GetComponent<Rigidbody>().useGravity = true;
                }
                g.transform.parent = null;
                g.GetComponent<Collider>().enabled = true;
                Destroy(g, 10f);
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player" && state == EnemyState.CHASE)
        {
            if(!shooter)state = EnemyState.ATTACK;
        }
        else if (col.gameObject.tag == "playerBullet")
        {
            anim.SetTrigger("Damage");

        }

        if (col.gameObject.tag == "GameBoundaries")
        {
            Destroy(gameObject);

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && state == EnemyState.GUARD)
        {
            oldTargetPos = other.transform.position;
            oldPosition = transform.position;
            target = other.transform;
            state = EnemyState.CHASE;
        }
        else if (other.tag == "Waypoint" && state == EnemyState.GUARD)
        {
            curWaypoint++;
            if (curWaypoint >= waypoints.Count)
            {
                curWaypoint = 0;
            }
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && (state == EnemyState.CHASE || state == EnemyState.EVADE))
        {
            target = waypoints[curWaypoint];
            state = EnemyState.GUARD;
        }
    }
}
