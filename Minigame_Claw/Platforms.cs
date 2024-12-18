using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    private int i;
    public Transform claw;

    private void Start()
    {
        transform.position = points[startingPoint].position;
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if(i == points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            other.transform.SetParent(transform);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
        {
            other.transform.SetParent(claw);
        }

    }

}
