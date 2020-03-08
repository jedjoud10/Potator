using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
public class FrierWeaponScript : MonoBehaviour
{
    public Transform barreloutput;//The part where the potato comes out
    public GameObject potatoPrefab;//The potato that is going to be shot
    private bool isShooting;//Are we shooting
    private PlayerInputActions inputActions;
    // Start is called before the first frame update
    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.PlayerControls.Shoot.performed += ctx => isShooting = ctx.ReadValueAsButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting) //Shoot the bullet
        {
            GameObject spawnedpotato = Instantiate(potatoPrefab, barreloutput);
            spawnedpotato.GetComponent<NetworkedObject>().Spawn();
        }
    }
}
