using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {

    public enum PlayerState { Alive, Dead, Paused};

    public PlayerState state = PlayerState.Alive;

    //Attached Player Objects
    public GameObject bullet;
    public List<GameObject> barrels;
    public GameObject shieldObject;
    public MissileControl missileControl;

    //Control Variables
    //Movement Variables
    public Vector3 movement;
    public float rotate;
    public float curRotation = 0;
    public float speed = 10f;
    public float rotationSpeed = 5f;

    //Attack Variables
    public bool shoot = false;
    public float timeBetweenShots = 0.5f;
    float counter = 0;
    int barrelIndex = 0;
    int lastBarrage = 0;
    public int missileBarrageCost = 200;
    public bool missileBarrage = false;

    //Health Variables
    public float maxShield = 100f;
    public float shield = 100f;
    public float health = 100f;
    public float maxHealth = 100f;
    public float healTimer = 5f;
    float healCounter = 0;

    //Components
    Animator anim;
    Animator damageScreen;
    Animator shieldAnim;

    //External gameObjects/components
    CanvasController canvas;


    private void Awake()
    {
        Physics.IgnoreLayerCollision(11, 12);
        Physics.IgnoreLayerCollision(11, 9);
    }
    // Use this for initialization
    void Start ()
    {
        anim = GetComponentInChildren<Animator>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>(); ;
        damageScreen = GameObject.FindGameObjectWithTag("DamageScreen").GetComponent<Animator>();
        shieldAnim = shieldObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch(state)
        {
            case PlayerState.Alive:
                {
                    GetInput();
                    Move();
                    Rotate();
                    Counters();
                    MissileVariables();
                   
                    break;                  
                }

            case PlayerState.Dead:
                {
                    canvas.ShowDeathScreen();
                    break;
                }
            case PlayerState.Paused:
                {
                    canvas.ShowPauseScreen();
                    if (Input.GetKeyDown(KeyCode.P)) state = PlayerState.Alive;
                    break;
                }
        }
	}

    void GetInput()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");
        rotate = Input.GetAxis("Rotation");
        shoot = Input.GetKey(KeyCode.Mouse0);
        if (Input.GetKeyDown(KeyCode.P)) state = PlayerState.Paused;
    }

    void Move()
    {
        anim.SetFloat("xAxis", movement.x);
        anim.SetFloat("zAxis", movement.z);

        transform.Translate(movement * Time.deltaTime * speed);
    }

    void Rotate()
    {
        if (rotate != 0)
        {
            curRotation += rotate * rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, transform.rotation.y + curRotation, 0);
        }
    }

    void Counters()
    {
        if (counter < timeBetweenShots)
        {
            counter += Time.deltaTime;
        }

        if (healCounter < healTimer)
        {
            healCounter += Time.deltaTime;
        }
        else RegenerateHealth();

        if (shoot && counter >= timeBetweenShots)
        {
            Shoot();
        }
    }

    void RegenerateHealth()
    {

        if (shield < maxShield)
        {
            shield += 2 * Time.deltaTime;
            if(shield >= 0) shieldObject.SetActive(true);
            if (shield >= maxShield)
            {
                shield = maxShield;
            }
        }

        if (health < maxHealth)
        {
            health += 1 * Time.deltaTime;
            if (health >= maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    void Shoot()
    {
        if (shoot)
        {
            Instantiate(bullet, barrels[barrelIndex].transform.position, barrels[barrelIndex].transform.rotation);
            barrels[barrelIndex].GetComponent<AudioSource>().Play();
            GetComponent<Rigidbody>().AddForce(transform.forward * -100);
            counter = 0;
            barrelIndex++;
            if(barrelIndex >= 3)
            {
                barrelIndex = 0;
            }
        }
    }


    void MissileVariables()
    {
        if (GameSettings.instance.curScore - lastBarrage >= missileBarrageCost)
        {
            missileBarrage = true;
            canvas.GetComponent<CanvasController>().SetMissileTextActive(missileBarrage);
        }

        if (Input.GetKey(KeyCode.M) && missileBarrage == true)
        {
            missileControl.SpawnMissiles();
            lastBarrage = GameSettings.instance.curScore;
            missileBarrage = false;
            canvas.GetComponent<CanvasController>().SetMissileTextActive(missileBarrage);
        }
    }

    public void TakeDamage(float damage)
    {
        damageScreen.SetTrigger("Damaged");
        healCounter = 0;
        if (shield >= 0)
        {
            shield -= damage;
            shieldAnim.SetTrigger("hit");

        }
        else
        {
            health -= damage;
        }

        if (shield <= 0)
        {
            shieldObject.SetActive(false);
        }

        if (health <= 0)
        {
            state = PlayerState.Dead;
        }
    }
}

