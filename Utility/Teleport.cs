using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform newLocation;
    public GameObject player;
    public void MovePlayer()
    {
        player.transform.position = newLocation.position;
    }
}
