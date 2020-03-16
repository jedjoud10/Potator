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
    private GameObject cameraobject;//the camera of the player
    private PlayerInputActions inputAction;//Input actions
    private Vector2 movementInput;//Movement input of player
    private Vector2 movement;//The movement of the player to apply to the rigidbody 2d
    private Vector3 movementVector;//Vector3D of the movement of the player in 2D
    public GameObject cameraHolder;//Object that will hold the main camera
    private PlanetScript[] planets;//The planets in the scene
    public float mass;//Player mass

    public GameObject playerDir1;//Whne the player walks forward
    public GameObject playerDir2;//When the player walks backward

    public Text usernameText;//Text box showing the username of the player

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
        else 
        {
            SetUsername(username.Value);
        }
        planets = GameObject.FindObjectsOfType<PlanetScript>();//Init planets
    }

    // Update is called once per frame
    void Update()
    {
        //Init full movement values
        if (!IsLocalPlayer) { return;  }
        movement.x = movementInput.x * speed;
        movement.y = movementInput.y * speed;
        movementVector = transform.TransformDirection(movement);      
        if(movement.x < 0) 
        {
            playerDir1.SetActive(false);
            playerDir2.SetActive(true);
        }
        else 
        {
            playerDir1.SetActive(true);
            playerDir2.SetActive(false);
        }
    }
    //Update for physics
    void FixedUpdate()
    {
        if (!IsLocalPlayer) { return; }
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].Attract(rb, mass);//Calculate custom gravity using Newton's force formual
        }
        rb.AddForce(new Vector2(movementVector.x, movementVector.y));
    }
    //Set new username for this player
    public void SetUsername(string _username) 
    {
        usernameText.text = _username;
        InvokeServerRpc(SetUsernameServer, _username, this);
    }
    //Set new username server
    [ServerRPC]
    private void SetUsernameServer(string _username, PlayerControllerScript _player)
    {
        username.Value = _username;
        InvokeClientRpcOnEveryone(SetUsernameClient, _username, _player);
    }
    //Set new username on all clients
    [ClientRPC]
    private void SetUsernameClient(string _username, PlayerControllerScript _player)
    {
        if (!IsLocalPlayer)
        {
            _player.usernameText.text = _username;
        }
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
}
