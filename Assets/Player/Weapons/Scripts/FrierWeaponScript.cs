using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class FrierWeaponScript : NetworkedBehaviour
{
    public Transform barreloutput;//The part where the potato comes out
    public GameObject potatoPrefab;//The potato that is going to be shot
    private bool isShooting;//Are we shooting
    private PlayerInputActions inputAction;
    private float time;//Time that we are shooting
    public float delaytime;//Delay between each shot
    private GameObject mycamera;//Camera of player
    private RaycastHit hit;//The hit out struct of the raycast

    void Awake()
    {
        inputAction = new PlayerInputActions();
        inputAction.PlayerControls.Shoot.performed += ctx => isShooting = ctx.ReadValueAsButton();
    }
    // Start is called before the first frame update
    private void Start()
    {
        mycamera = GameObject.FindObjectOfType<Camera>().gameObject;
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
                InvokeServerRpc(ServerSpawnPotato, barreloutput.position, barreloutput.rotation, barreloutput.forward);
                Debug.DrawRay(barreloutput.position, barreloutput.forward, Color.red, 1.0f);
            }
        }
        if (IsLocalPlayer)
        {
            if (Physics.Raycast(mycamera.transform.position, mycamera.transform.forward * 1000, out hit))
            {
                transform.LookAt(hit.point, mycamera.transform.up);//Make gun point at center of screen
            }
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
