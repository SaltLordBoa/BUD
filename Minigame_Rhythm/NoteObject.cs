using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{

    public bool canBePressed;
    public KeyCode keyToPress;

    
    void Update()
    {
        if(Input.GetKeyDown(keyToPress))
        {
            if(canBePressed)
            {
                gameObject.SetActive(false);

                RhythmGameManager.instance.NoteHit();
                RhythmGameManager.instance.ChangeImage();
            }
        }
        if(Input.GetKeyUp(keyToPress))
        {
            RhythmGameManager.instance.BaseImage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Key")
        {
            canBePressed = true;         
        }
        if(other.tag == "Ledge")
        {
            RhythmGameManager.instance.BaseImage();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Key" && gameObject.activeSelf)
        {
            canBePressed = false;

            RhythmGameManager.instance.NoteMissed();
        }
    }
}
