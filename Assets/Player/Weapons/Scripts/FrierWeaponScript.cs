using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
public class FrierWeaponScript : MonoBehaviour
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
        if (isShooting) //Shoot the bullet
        {
            time += Time.deltaTime;
            if (time >= delaytime)
            {
                time = 0;
                GameObject spawnedpotato = Instantiate(potatoPrefab, barreloutput.position, barreloutput.rotation);
                spawnedpotato.GetComponent<NetworkedObject>().Spawn();
            }
        }
        if (Physics.Raycast(mycamera.transform.position, mycamera.transform.forward * 100000, out hit))
        {
            transform.LookAt(hit.point, mycamera.transform.up);//Make gun look at middle of screen so we can use crosshair efficently
            Debug.DrawRay(mycamera.transform.position, mycamera.transform.forward * 100000);
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
