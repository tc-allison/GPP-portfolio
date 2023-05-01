using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour
{
    private GameObject player;
    private Animator animator;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = this.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            animator.SetTrigger("VanishTrigger");
            StartCoroutine(DestroyPlatform());
        }
    }

    IEnumerator DestroyPlatform()
    {
        yield return new WaitForSeconds(2.2f);
        player.transform.parent = null;
        this.gameObject.SetActive(false);
    }

}
