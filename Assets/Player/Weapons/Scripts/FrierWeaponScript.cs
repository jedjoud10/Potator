using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class FrierWeaponScript : NetworkedBehaviour
{
    public Transform crosshair;//Crosshair opf the local player
    public Transform player;//Our player
    private Rigidbody2D playerRigidbody;//The rigidbody of the player
    public Transform barreloutput;//The part where the potato comes out
    public GameObject potatoPrefab;//The potato that is going to be shot
    public GameObject shootparticles;//The poarticles that aere spawned when you shoot the frier
    private bool isShooting;//Are we shooting
    private Vector2 mousePosition;//The mouse position on the screen
    private PlayerInputActions inputAction;
    private float time;//Time that we are shooting
    public float delaytime;//Delay between each shot
    private Camera mycamera;//Camera of player

    void Awake()
    {
        inputAction = new PlayerInputActions();
        inputAction.PlayerControls.Shoot.performed += ctx => isShooting = ctx.ReadValueAsButton();
        inputAction.PlayerControls.Camera.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        mycamera = GameObject.FindObjectOfType<Camera>();
        if (IsLocalPlayer) 
        {
            playerRigidbody = player.GetComponent<Rigidbody2D>();
            crosshair.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting && IsLocalPlayer) //Shoot the bullet
        {
            time += Time.deltaTime;
            if (time >= delaytime)
            {
                time = 0;
                playerRigidbody.AddForceAtPosition(-transform.right * 1000, transform.position);
                InvokeServerRpc(ServerSpawnPotato, barreloutput.position, barreloutput.rotation, barreloutput.forward);
                Debug.DrawRay(barreloutput.position, barreloutput.forward, Color.red, 1.0f);
            }
        }
        if (IsLocalPlayer)
        {
            Vector3 worldpos = mycamera.ScreenToWorldPoint(mousePosition);//Get world position from cursor mouse position
            worldpos.z = transform.position.z;//Reset z value of the world position
            transform.LookAt(worldpos, player.transform.up);//Make gun point at center of screen    
            transform.position = new Vector3(transform.position.x, transform.position.y, -5.0f);            
            worldpos.z = -5;
            crosshair.transform.position = worldpos;
        }
    }
    //Spawn potato on server
    [ServerRPC]
    private void ServerSpawnPotato(Vector3 pos, Quaternion rot, Vector3 forward)
    {
        GameObject spawnedpotato = Instantiate(potatoPrefab, pos, rot);
        spawnedpotato.transform.rotation = rot;
        spawnedpotato.GetComponent<PotatoScript>().forward = forward;
        spawnedpotato.GetComponent<NetworkedObject>().Spawn();
        GameObject spawnedparticles = Instantiate(shootparticles, pos, rot);
        spawnedparticles.transform.forward = forward;
        spawnedparticles.GetComponent<NetworkedObject>().Spawn();
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
