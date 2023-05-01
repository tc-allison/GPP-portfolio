using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpTimerUI : MonoBehaviour
{
    public PlayerLocomotion locomotion;
    public float currentDoubleJumpTimeUI = 0f;
    public float currentSpeedTimeUI;

    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI countdownText2;


    void Awake()
    {
        locomotion = GameObject.Find("RPG-Character (1)").GetComponent<PlayerLocomotion>();
        countdownText.gameObject.SetActive(false);
        countdownText2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(locomotion.maxJumps > 0) 
        {
            countdownText.gameObject.SetActive(true);
            currentDoubleJumpTimeUI = locomotion.currentDoubleJumpTime;
            countdownText.text = currentDoubleJumpTimeUI.ToString("0");
          
        }
        if(locomotion.speedPickedUp)
        {
            countdownText2.gameObject.SetActive(true);
            currentSpeedTimeUI = locomotion.currentSpeedTime;
            countdownText2.text = currentSpeedTimeUI.ToString("0");
            
        }
       
        


    }
}
