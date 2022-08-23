using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //to access the library that contains UI and canvas components

public class PlayerHandler : MonoBehaviour
{
    public float jumpHeight = 5f;
    public float climbSpeed = 5f;
    public float moveSpeed = 10f;
    public float dashSpeed = 20f;
    private CharacterController2D controller;
    // Start is called before the first frame update
    public int curHealth = 3;
    public Transform respawn;
    public GameObject heart;
    //public GameObject[] heartDisplay;
    public List<GameObject> heartDisplay = new List<GameObject>();
    public Transform heartContainer;
    public static bool stronk;
    public float deathHeight = -30;
    private bool fallDeath;
    private bool haveKey = false;
    public float tookDamage;
    public float timerScore = 1000;

    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        respawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>();
        for (int i = 0; i < curHealth; i++)
        {
            GameObject currentHeart = Instantiate(heart, heartContainer);
            heartDisplay.Add(currentHeart);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        bool isJumping = Input.GetButtonDown("Jump");
        //bool isDashing = Input.GetButtonDown("Fire1");
        if (Input.GetButtonDown("Fire1"))
        {
            controller.Dash(dashSpeed);
        }
        bool isRunning = inputH != 0;
        if (isJumping)
        {
            controller.daveJump(jumpHeight);
        }
        float horizontal = inputH * moveSpeed;
        float vertical = inputV * climbSpeed;

        controller.Climb(vertical);
        controller.Move(horizontal);
        if (curHealth == 0 && GameManager.currentGameState == GameStates.Game)
        {
            GameManager.currentGameState = GameStates.PostGame;
        }
        if (this.transform.position.y <= deathHeight)
        {
            if (!fallDeath)
            {
                curHealth--;
                controller.anim.SetTrigger("IsDead");
                //move and respawn character
                //change UI
                if (heartDisplay.Count > 0)
                {
                    Destroy(heartDisplay[heartDisplay.Count - 1]);
                    heartDisplay.RemoveAt(heartDisplay.Count - 1);
                }
                stronk = false;
                fallDeath = true;
            }
        }
        if (tookDamage > 0)
        {
            tookDamage -= Time.deltaTime;
        }
        timerScore -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickup"))
        {
            if (tookDamage > 0)
            {
                //no cheating
                //only pick up score items while damagable
            }
            else
            { 
            Destroy(other.gameObject);
            GameManager.score += other.GetComponent<Pickup>().value; // score = score + value
            GameManager.scoreText.text = "" + GameManager.score;
            }
        }
        if (other.CompareTag("Health"))
        {
            curHealth++;
            GameObject currentHeart = Instantiate(heart, heartContainer);
            heartDisplay.Add(currentHeart);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("PowerUp"))
        {
            stronk = true;

            Destroy(other.gameObject);
        }
        if (other.CompareTag("Damage"))
        {
            if (!stronk)
            {
                if (tookDamage <= 0)
                {
                    curHealth--;
                    controller.anim.SetTrigger("IsDead");
                    //move and respawn character
                    //change UI
                    if (heartDisplay.Count > 0)
                    {
                        Destroy(heartDisplay[heartDisplay.Count - 1]);
                        heartDisplay.RemoveAt(heartDisplay.Count - 1);
                    }
                    tookDamage = 1;
                }
            }
            else
            {
                stronk = false;
            }
        }
        if (other.CompareTag("Finish"))
        {
            controller.anim.SetTrigger("IsWin");
            GameManager.score += 100;
            //timer bonus
            if (timerScore > 0)
            {
                GameManager.score += (int)(timerScore/10);
            }
            controller.Stop();
            GameManager.currentGameState = GameStates.PostGame;
            stronk = false;
        }
        //key and door stuff
        if (other.CompareTag("Key"))
        {
            haveKey = true;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Door"))
        {
            if (haveKey == true)
            {
                Destroy(other.gameObject);
            }
        }
        if (other.CompareTag("Lever"))
        {
            if(other.GetComponent<Lever>())
            {
                other.GetComponent<Lever>().Triggered();
            }
        }
    }
    public void Respawn()
    {
        transform.position = respawn.position;
        fallDeath = false;
    }
}