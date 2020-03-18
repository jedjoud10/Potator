using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
//Script for de potate
public class PotatoScript : NetworkedBehaviour
{
    public GameObject playerprefab;//The player prefab that will be respawned when we kill a player
    private Rigidbody2D rb;//The rigidbody of de potatoeeoe
    public float speed;//Speed of de potatoeoe
    public float rotationForwardSpeed;//How fast the potato looks at the forward vector
    public Vector3 forward;//Forward vector for force to add to potato
    public GameObject particles;//The particle system that is going to be spawned when the potato crashes ;-;
    public int damage;//How much damage this potato does
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(forward * speed);
        Destroy(gameObject, 10.0f);//Potatoes dont live forever ;-;
        transform.rotation = Quaternion.LookRotation(forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity != null || rb.velocity != Vector2.zero) 
        {
            float rot_z = Mathf.Atan2(rb.velocity.normalized.y, rb.velocity.normalized.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }
    //When we hit something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerHealthScript>() != null)//If we hit a player 
        {
            PlayerHealthScript playerHealthScript = collision.gameObject.GetComponent<PlayerHealthScript>();
            playerHealthScript.DamagePlayer(damage);
        }
        InvokeServerRpc(SpawnParticlesServer);       
    }
    //Spawn particles
    [ServerRPC]
    private void SpawnParticlesServer() 
    {
        GameObject spawnedparticles = Instantiate(particles, transform.position, transform.rotation);
        spawnedparticles.GetComponent<NetworkedObject>().Spawn();
        Destroy(gameObject);//Destroy the potato since it hit something
    }
}
