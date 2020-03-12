using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Scrip that handles gravity calculations for planets but only for other objects other than the player
public class PlanetRigidbodyScript : MonoBehaviour
{
    public float mass;//Planet mass
    private const float gravitationalConstant = -10f;//Constant multiplicator for gravity force
    public void Start()
    {
        mass = transform.localScale.magnitude * 30;
    }
    public void Attract(Rigidbody2D rb, float rbMass) //Attract the player to the planet
    {
        Vector2 direction = rb.position - new Vector2(transform.position.x, transform.position.y);//Direction from player to planet center        
        float distance = direction.magnitude;//Distance between player and planet

        float forceStrengh = ((rbMass * mass) / (distance * distance)) * gravitationalConstant;
        Vector2 force = direction.normalized * forceStrengh;
        rb.AddForce(force * Time.deltaTime);//Add custom gravity
    }
}
