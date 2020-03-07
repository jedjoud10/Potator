using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;
//Script that handles UI in MainMenu
public class MainMenuUIScript : MonoBehaviour
{
    public InputField username_InputFiel;//The inputfield for the username of the player
    public InputField IP_InputField;//The inputfield for the IP of server
    public GameObject MainMenuUI;//The whole MainMenu canvas UI
    public NetworkingManager networkManager;//The network manager for the game scene
    public UnetTransport unetTransport;//Script on the network manager that allows us to change the IP of the server that we are going to connect to
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Join server with specific IP
    public void JoinServer() 
    {
        if (IP_InputField.text != "")
        {
            unetTransport.ConnectAddress = IP_InputField.text;//Set IP
        }
        else 
        {
            unetTransport.ConnectAddress = "127.0.0.1";//Set default localhost IP
        }
        networkManager.StartClient();//Connect to server
        MainMenuUI.SetActive(false);//Hide UI
    }
    //Host server
    public void HostServer() 
    {
        networkManager.StartHost();//Start hosting server
        MainMenuUI.SetActive(false);//Hide UI
    }
}
