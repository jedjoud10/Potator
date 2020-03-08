using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Scrip that handles gravity calculations for planets but only for other objects other than the player
public class PlanetRigidbodyScript : MonoBehaviour
{
    public float mass;//Planet mass
    private const float gravitationalConstant = -1.0f;//Constant multiplicator for gravity force
    public void Attract(Rigidbody rb, float rbMass) //Attract the player to the planet
    {
        Vector3 direction = rb.position - transform.position;//Direction from player to planet center        
        float distance = direction.magnitude;//Distance between player and planet

        float forceStrengh = ((rbMass * mass) / (distance * distance)) * gravitationalConstant;
        Vector3 force = direction.normalized * forceStrengh;
        rb.AddForce(force);//Add custom gravity
    }
}
