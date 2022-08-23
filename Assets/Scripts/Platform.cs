using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Collider2D platform;
    public Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void Update()
    {
        if (platform.enabled && transform.position.y > player.position.y)
        {
            platform.enabled = false;
        }
        if (!platform.enabled && (transform.position.y+1 < player.position.y))
        {
            platform.enabled = true;
        }
    }
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player") && transform.position.y > other.transform.position.y)
    //    {
    //        platform.enabled = true;
    //    }
    //}
}