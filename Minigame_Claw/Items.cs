using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public PlatformManager gameManager;
    public GameObject item;
    public int objectScoreAmount;
    public bool bonus;

    private void Awake()
    {
        item = this.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Claw")
        {
            this.transform.SetParent(other.transform);
            Debug.Log("grab");
        }

        if (other.tag == "Drop")
        {
            this.gameObject.transform.SetParent(null);
            Destroy(this.gameObject);
            gameManager.pickupList.Remove(item);
            gameManager.IncreaseScore(objectScoreAmount);
            if(bonus == true)
            {
                gameManager.GameOver();
            }
        }

    }

    public void ReleaseObject()
    {
        this.gameObject.transform.SetParent(null);
    }

}
