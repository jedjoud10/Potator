using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script that controls the client player
public class PlayerControllerScript : MonoBehaviour
{
    private Rigidbody rb;//The rigidbody that the player has for physics
    public float speed;//The walking speed of the player
    public GameObject cameraobject;//the camera of the player
    private float cameraRotationX;//The rotation of the camera in an up/down axis
    public float sensivity;//The sensivity of the camera rotation 
    private PlayerInputActions inputAction;//Input actions
    private Vector2 movementInput;//Movement input of player
    private Vector3 movement;//The movement of the player with Y axis
    private Vector2 cameraRotation;//Rotation of camera
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
    }

    // Update is called once per frame
    void Update()
    {
        //Init full movement values
        movement.x = movementInput.x;
        movement.z = movementInput.y;
        movement.y = 0;//Jump still needs to be worked on
        Debug.Log(movement);
        rb.MovePosition(rb.position + transform.TransformDirection(movement * speed * Time.deltaTime));
        cameraRotationX -= cameraRotation.y * sensivity;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);
        cameraobject.transform.localEulerAngles = new Vector3(cameraRotationX, 0, 0);
        transform.Rotate(new Vector3(0, cameraRotation.x * sensivity), Space.Self);
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
