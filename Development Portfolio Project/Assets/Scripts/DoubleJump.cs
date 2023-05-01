using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public PlayerLocomotion locomotion;

    
    void Awake()
    {
        locomotion = GameObject.Find("RPG-Character (1)").GetComponent<PlayerLocomotion>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            locomotion.maxJumps = 1;
            Destroy(this.gameObject);
        }
    }
}
