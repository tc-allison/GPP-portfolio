using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public PlayerLocomotion locomotion;


    void Awake()
    {
        locomotion = GameObject.Find("RPG-Character (1)").GetComponent<PlayerLocomotion>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            locomotion.speedPickedUp = true;
            locomotion.walkingSpeed = locomotion.walkPowerUpSpeed;
            locomotion.runningSpeed = locomotion.runPowerUpSpeed;
            locomotion.sprintingSpeed = locomotion.sprintPowerUpSpeed;
            Destroy(this.gameObject);
        }
    }
}
