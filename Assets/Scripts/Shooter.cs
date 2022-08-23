using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bullet;
    
    public float fireRate;
    public float timer;
    public Vector2[] direction = new Vector2[4];
    public int chosenDirection;
    public bool spin;
    public float speed = 1000;
    public bool isRandom;
    public bool isEnabled = true;
    //Update is called once per frame
    private void Start()
    {
        //fireRate = Random.Range(1f, 4f);
        direction[0] = Vector2.up;
        direction[1] = Vector2.down;
        direction[2] = Vector2.left;
        direction[3] = Vector2.right;
    }

    void Update()
    {
        if (isEnabled)
        {
            timer += Time.deltaTime;
            if (timer >= fireRate)
            {
                if (spin)
                {
                    chosenDirection = Random.Range(0, direction.Length);
                }
                GameObject currentSpawn = Instantiate(bullet, this.transform);
                currentSpawn.GetComponent<Rigidbody2D>().gravityScale = 0;
                currentSpawn.GetComponent<Rigidbody2D>().velocity = (direction[chosenDirection] * speed * Time.deltaTime);
                Destroy(currentSpawn, 5);
                timer = 0;
                if (isRandom)
                {
                    fireRate = Random.Range(1f, 4f);
                }
            }
        }
    }
}