using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{

    
    
    public PlayerLocomotion locomotion;
    public GameObject playerCamera;
    public GameObject CutsceneCamera;
    public GameObject CutsceneCamera2;
    public GameObject timeline;
    public Rigidbody rb;


    void Awake()
    {
        CutsceneCamera.SetActive(false);
        CutsceneCamera2.SetActive(false);
        timeline.SetActive(false);
        playerCamera.SetActive(true);
        locomotion = GameObject.Find("RPG-Character (1)").GetComponent<PlayerLocomotion>();
        rb = GameObject.Find("RPG-Character (1)").GetComponent<Rigidbody>();
    }

    IEnumerator FinishCut()
    {
        yield return new WaitForSeconds(7);
        playerCamera.SetActive(true);
        CutsceneCamera.SetActive(false);
        CutsceneCamera2.SetActive(false);
        timeline.SetActive(false);
        gameObject.SetActive(false);
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            locomotion.insideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            locomotion.insideTrigger = false;
        }
    }


    private void FixedUpdate()
    {
        if (locomotion.doorInteractTrigger)
        {
            locomotion.insideTrigger = false;
            playerCamera.SetActive(false);
            timeline.SetActive(true);
            rb.constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(FinishCut());
            
            
        }    
    }



}
