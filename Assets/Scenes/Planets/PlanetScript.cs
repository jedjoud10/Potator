using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script that handles gavity calculations for planets
public class PlanetScript : MonoBehaviour
{
    public float mass;//Planet mass
    private const float gravitationalConstant = -0.2f;//Constant multiplicator for gravity force
    private const float gravityRotationSpeed = 1.0f;//How fast the player correctly matches up with the ground rotation
    public void Attract(Rigidbody player, float playerMass) //Attract the player to the planet
    {
        Vector3 direction = player.position - transform.position;//Direction from player to planet center        
        float distance = direction.magnitude;//Distance between player and planet

        float forceStrengh = ((playerMass * mass) / (distance * distance)) * gravitationalConstant;
        Vector3 force = direction.normalized * forceStrengh;
        player.AddForce(force);//Add custom gravity

        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, direction.normalized) * player.rotation;//The rotation we want to get at
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, gravityRotationSpeed * Time.deltaTime * -forceStrengh);//Smoothed out rotation
    }
}
