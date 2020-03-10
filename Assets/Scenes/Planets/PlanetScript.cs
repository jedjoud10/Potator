using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script that handles gavity calculations for planets
public class PlanetScript : MonoBehaviour
{
    public float mass;//Planet mass
    private const float gravitationalConstant = -7f;//Constant multiplicator for gravity force
    private const float gravityRotationSpeed = 0.001f;//How fast the player correctly matches up with the ground rotation
    public void Attract(Rigidbody2D player, float playerMass) //Attract the player to the planet
    {
        Vector2 direction = player.position - new Vector2(transform.position.x, transform.position.y);//Direction from player to planet center  
        Vector3 direction3D = new Vector3(direction.x, direction.y, 0);
        float distance = direction.magnitude;//Distance between player and planet

        float forceStrengh = ((playerMass * mass) / (distance * distance)) * gravitationalConstant;
        Vector2 force = direction.normalized * forceStrengh;
        player.AddForce(force * Time.deltaTime);//Add custom gravity


        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, direction.normalized) * player.transform.rotation;//The rotation we want to get at
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime  * -forceStrengh * gravityRotationSpeed);//Smoothed out rotation
    }
}
