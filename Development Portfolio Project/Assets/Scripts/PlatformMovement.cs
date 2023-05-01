using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public GameObject startPos;
    public GameObject destinationPos;

    public float speed = 3.0f;
    private bool to = true;
    private float elapsedTime = 0;

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        float step = speed * Time.deltaTime;
        if (elapsedTime > 2.0f)
        {
            if (to == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinationPos.transform.position, step);
            }
            if (to == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos.transform.position, step);

            }
        }
        if(to==true)
        {
            if (transform.position == destinationPos.transform.position)
            {
                to = false;
                elapsedTime = 0;
            }
        }
        if(to == false)
        {
            if (transform.position == startPos.transform.position)
            {
                to = true;
                elapsedTime = 0;

            }
        }
    }

 }   
