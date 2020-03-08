using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
//Script for de potate
public class PotatoScript : MonoBehaviour
{
    public GameObject playerprefab;//The player prefab that will be respawned when we kill a player
    private PlanetRigidbodyScript[] planets;//Planets for gravity calculations
    private Rigidbody rb;//The rigidbody of de potatoeeoe
    public float speed;//Speed of de potatoeoe
    public float mass;//Mass of de potate
    public float rotationForwardSpeed;//How fast the potato looks at the forward vector
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        planets = GameObject.FindObjectsOfType<PlanetRigidbodyScript>();
        rb.AddForce(transform.forward * speed);
        Destroy(gameObject, 10.0f);//Potatoes dont live forever ;-;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity.normalized), rotationForwardSpeed * Time.deltaTime);
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
            //Kill the hit player
            Destroy(collision.gameObject);
            GameObject spawnedplayer = Instantiate(playerprefab, Vector3.zero, Quaternion.identity);
            spawnedplayer.GetComponent<NetworkedObject>().Spawn();
        }
        Destroy(gameObject);//Destroy the potato since it hit something
    }
}
