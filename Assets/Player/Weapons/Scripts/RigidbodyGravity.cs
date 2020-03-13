using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script for custom gravity for this rigidbody form planets 
public class RigidbodyGravity : MonoBehaviour
{
    public float mass;//Mass of thius rigidbody
    private Rigidbody2D rb;//Our rigidbody
    private PlanetRigidbodyScript[] planets;//Planets for gravity calculations
    // Start is called before the first frame update
    void Start()
    {
        planets = GameObject.FindObjectsOfType<PlanetRigidbodyScript>();
        rb = GetComponent<Rigidbody2D>();//Init rigidbody
    }

    //Physics update
    private void FixedUpdate()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].Attract(rb, mass);//Calculate custom gravity using Newton's force formual
        }
    }
}
