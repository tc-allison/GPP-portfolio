using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemyHandler : MonoBehaviour
{
    public Collider weaponCollider;
    public PlayerLocomotion locomotion;
    public MeshRenderer meshRenderer;
    public Rigidbody rb;
    public GameObject player;
    public GameObject enemy;
    public GameObject parentGameObject;
    public GameObject slimeParticles;
    Color originalColour;
    float flashTime = .15f;
    public int hp = 30;
    public float strength = 16, delay = 0.15f;


    void Awake()
    {
        weaponCollider = GameObject.Find("HitCollider").GetComponent<Collider>();
        locomotion = GameObject.Find("RPG-Character (1)").GetComponent<PlayerLocomotion>();
        parentGameObject = this.transform.parent.gameObject;
        meshRenderer = parentGameObject.GetComponent<MeshRenderer>();
        player = GameObject.Find("RPG-Character (1)");
        originalColour = meshRenderer.material.color;

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other == weaponCollider && locomotion.isAttacking)
        {
            hp -= 10;
            FlashStart();
            PlayFeedback(player);
        }
    }

    private void FixedUpdate()
    {
        if (hp == 0)
        {
            GameObject.Destroy(parentGameObject);
        }
    }

    void FlashStart()
    {
        meshRenderer.material.color = Color.red;
        slimeParticles.GetComponent<ParticleSystem>().Play();
        Invoke("FlashStop", flashTime);

    }

    void FlashStop()
    {
        meshRenderer.material.color = originalColour;
    }

    public void PlayFeedback(GameObject player)
    {
        StopAllCoroutines();
        Vector3 direction = (transform.position - player.transform.position).normalized;
        rb.AddForce(direction * strength, ForceMode.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
    }
}
