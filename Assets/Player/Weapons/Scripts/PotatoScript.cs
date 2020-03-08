using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
//Script for de potate
public class PotatoScript : NetworkedBehaviour
{
    public GameObject playerprefab;//The player prefab that will be respawned when we kill a player
    private PlanetRigidbodyScript[] planets;//Planets for gravity calculations
    private Rigidbody rb;//The rigidbody of de potatoeeoe
    public float speed;//Speed of de potatoeoe
    public float mass;//Mass of de potate
    public float rotationForwardSpeed;//How fast the potato looks at the forward vector
    public Vector3 forward;//Forward vector for force to add to potato
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        planets = GameObject.FindObjectsOfType<PlanetRigidbodyScript>();
        rb.AddForce(forward * speed);
        Destroy(gameObject, 10.0f);//Potatoes dont live forever ;-;
        transform.rotation = Quaternion.LookRotation(forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity != null || rb.velocity != Vector3.zero) 
        { 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity.normalized), rotationForwardSpeed * Time.deltaTime);
        }
        for(int i = 0; i < planets.Length; i++) 
        {
            planets[i].Attract(rb, mass);//Calculate custom gravity using Newton's force formual for potates/rigidbodies only and not player
        }
    }
    //When we hit something
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<PlayerControllerScript>() != null)//If we hit a player 
        {
            //Reset player
            GameObject player = collision.gameObject;
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.identity;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            InvokeServerRpc(PlayerReset, player);
        }
        Destroy(gameObject);//Destroy the potato since it hit something
    }
    //Reset player
    [ServerRPC]
    private void PlayerReset(GameObject player) 
    {
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
