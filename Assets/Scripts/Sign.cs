using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    public GameObject signDialogue;
    public Text signTextDisplay;
    public bool multiLine;
    public bool thisIsRealThisIsMe;
    public string signTextSingle;
    //multi lines
    int currentLineIndex = 0;
    public string[] signTextMulti;

    private void Update()
    {
        if (multiLine && signDialogue.activeSelf && thisIsRealThisIsMe)
        {
            if (Input.GetKeyDown(KeyCode.E) && currentLineIndex != signTextMulti.Length - 1)
            {
                currentLineIndex++;
                signTextDisplay.text = signTextMulti[currentLineIndex];
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            thisIsRealThisIsMe = true;
            signDialogue.SetActive(true);
            if (!multiLine)
            {
                signTextDisplay.text = signTextSingle;
            }
            else
            {
                signTextDisplay.text = signTextMulti[currentLineIndex];
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            thisIsRealThisIsMe = false;
            signDialogue.SetActive(false);
            currentLineIndex = 0;
        }
    }
}
