using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script that handles gavity calculations for planets
public class PlanetScript : MonoBehaviour
{
    public float mass;//Planet mass
    private const float gravitationalConstant = -1f;//Constant multiplicator for gravity force
    private const float gravityRotationSpeed = 0.07f;//How fast the player correctly matches up with the ground rotation
    public float planetRotationRadiusThreshold;//How close we are to the planet so that we get planetary rotation
    public void Start()
    {
        mass = transform.localScale.magnitude * 10;
    }
    public void Attract(Rigidbody2D player, float playerMass) //Attract the player to the planet
    {
        Vector2 direction = player.position - new Vector2(transform.position.x, transform.position.y);//Direction from player to planet center  
        Vector3 direction3D = new Vector3(direction.x, direction.y, 0);
        float distance = direction.magnitude;//Distance between player and planet

        float forceStrengh = ((playerMass * mass) / (distance * distance)) * gravitationalConstant;
        Vector2 force = direction.normalized * forceStrengh;
        player.AddForce(force);//Add custom gravity

        if((player.transform.position - transform.position).magnitude < planetRotationRadiusThreshold) 
        {
            Vector3 targetRotation = direction3D;//The rotation we want to get at
            player.transform.up = Vector3.Lerp(player.transform.up, targetRotation, Time.deltaTime * Mathf.Abs(forceStrengh) * gravityRotationSpeed);//Smoothed out rotation
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, planetRotationRadiusThreshold);
    }
}
