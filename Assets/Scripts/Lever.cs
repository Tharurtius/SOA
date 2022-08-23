using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Shooter spawner;
    public SpriteRenderer mySprite;
    // Start is called before the first frame update
    private void Start()
    {
        mySprite = this.GetComponent<SpriteRenderer>();
    }
    public void Triggered()
    {
        if (spawner.isEnabled)
        {
            mySprite.flipX = !mySprite.flipX;
            spawner.isEnabled = !spawner.isEnabled;
        }
        //else
        //{
        //    spawner.enabled = true;
        //}
    }
}
