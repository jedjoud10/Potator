using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
//Script that controls the client player
public class PlayerControllerScript : NetworkedBehaviour
{
    private Rigidbody rb;//The rigidbody that the player has for physics
    public float speed;//The walking speed of the player
    private GameObject cameraobject;//the camera of the player
    public GameObject weapon;//Weapon gameobject
    private float cameraRotationX;//The rotation of the camera in an up/down axis
    public float sensivity;//The sensivity of the camera rotation 
    private PlayerInputActions inputAction;//Input actions
    private Vector2 movementInput;//Movement input of player
    private Vector3 movement;//The movement of the player with Y axis
    private Vector2 cameraRotation;//Rotation of camera
    public GameObject cameraHolder;//Object that will hold the main camera
    private PlanetScript[] planets;//The planets in the scene
    private void Awake()
    {
        //Init player input controls
        inputAction = new PlayerInputActions();
        inputAction.PlayerControls.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputAction.PlayerControls.Camera.performed += ctx => cameraRotation = ctx.ReadValue<Vector2>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();//Init rigidbody

        //Init correct cursor settings
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (IsLocalPlayer)
        {
            //Init camera for this player
            cameraobject = GameObject.FindGameObjectWithTag("MainCamera");
            cameraobject.tag = "Untagged";
            cameraobject.transform.parent = cameraHolder.transform;
            cameraobject.transform.localPosition = Vector3.zero;


            planets = GameObject.FindObjectsOfType<PlanetScript>();//Init planets
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Init full movement values
        movement.x = movementInput.x;
        movement.z = movementInput.y;
        movement.y = 0;//Jump still needs to be worked on
        rb.MovePosition(rb.position + transform.TransformDirection(movement * speed * Time.deltaTime));
        cameraRotationX -= cameraRotation.y * sensivity;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);
        if (cameraobject != null)
        {
            cameraobject.transform.localEulerAngles = new Vector3(cameraRotationX, 0, 0);
            transform.Rotate(new Vector3(0, cameraRotation.x * sensivity), Space.Self);
            weapon.transform.rotation = cameraobject.transform.rotation;
        }
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].Attract(rb, 10);//Calculate custom gravity using Newton's force formual
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
