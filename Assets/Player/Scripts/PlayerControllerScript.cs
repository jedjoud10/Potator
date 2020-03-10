using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkedVar;
using MLAPI.Messaging;
//Script that controls the client player
public class PlayerControllerScript : NetworkedBehaviour
{
    public NetworkedVar<string> username = new NetworkedVar<string>("Default Username");//The username of the player
    private Rigidbody2D rb;//The rigidbody that the player has for physics
    public float speed;//The walking speed of the player
    public float jumpHeight;//The height of jump fo the player
    private bool canJump;//If the player can jump
    private GameObject cameraobject;//the camera of the player
    private PlayerInputActions inputAction;//Input actions
    private Vector2 movementInput;//Movement input of player
    private Vector2 movement;//The movement of the player to apply to the rigidbody 2d
    private Vector3 movementVector;//Vector3D of the movement of the player in 2D
    public GameObject cameraHolder;//Object that will hold the main camera
    private PlanetScript[] planets;//The planets in the scene
    public float mass;//Player mass

    private void Awake()
    {
        //Init player input controls
        inputAction = new PlayerInputActions();
        inputAction.PlayerControls.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//Init rigidbody
        if (IsLocalPlayer)
        {
            //Init camera for this player
            cameraobject = GameObject.FindGameObjectWithTag("MainCamera");
            cameraobject.tag = "Untagged";
            cameraobject.transform.parent = cameraHolder.transform;
            cameraobject.transform.localPosition = Vector3.zero;
        }
        planets = GameObject.FindObjectsOfType<PlanetScript>();//Init planets
    }

    // Update is called once per frame
    void Update()
    {
        //Init full movement values
        movement.x = movementInput.x;        
        movementVector = transform.TransformDirection(movement * speed * Time.deltaTime);
        rb.MovePosition(rb.position + new Vector2(movementVector.x, movementVector.y));
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].Attract(rb, mass);//Calculate custom gravity using Newton's force formual
        }
        if (canJump)
        {
            //movement.y = movementInput.y * jumpHeight;
        }
        else 
        {
            //movement.y = 0;
        }       
    }
    //Set new username for this player
    public void SetUsername(string _username) 
    {
        username.Value = _username;
    }

    #region Enable/Disable
    private void OnEnable()
    {
        inputAction.Enable();//We started game, so enable them
    }
    private void OnDisable()
    {
        inputAction.Disable();//We finished game, so disable them
    }
    #endregion
    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        canJump = false;
    }
    #endregion
}
