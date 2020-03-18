using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkedVar;
using UnityEngine.UI;

public class PlayerHealthScript : NetworkedBehaviour
{
    public NetworkedVar<int> health;//Current health of player
    public int maxPlayerHealth;//The starting health of the player
    public Slider healthBar;//the health bar of the player

    // Start is called before the first frame update
    void Start()
    {
        SetHealth(maxPlayerHealth, maxPlayerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Take damage
    public void DamagePlayer(int _damage) 
    {
        if (IsLocalPlayer)
        {
            Debug.Log("Damage player");
            SetHealth(health.Value - _damage, maxPlayerHealth);
        }
    }
    //Set health of player
    private void SetHealth(int _health, int _maxHealth) 
    {
        healthBar.value = (float)_health / (float)_maxHealth;//Calculate percentage of health
        InvokeServerRpc(SetHealthServer, _health, _maxHealth, this, OwnerClientId);
    }
    //Set the health on server
    [ServerRPC]
    private void SetHealthServer(int _health, int _maxHealth, PlayerHealthScript _healthScript, ulong _clientId) 
    {
        health.Value = _health;
        if (_health <= 0) 
        {
            Respawn();
            InvokeClientRpcOnClient(RespawnClient , _clientId);
            health.Value = _maxHealth;
            _health = _maxHealth;
        }
        InvokeClientRpcOnEveryone(SetHealthClient, _health, _maxHealth, _healthScript);
    }
    //Set the health bar on all clients
    [ClientRPC]
    private void SetHealthClient(int _health, int _maxHealth,PlayerHealthScript _healthScript) 
    {
        _healthScript.healthBar.value = (float)_health / (float)_maxHealth;//Calculate percentage of health
    }
    //Respawn the player at center of the world. Called on server
    private void Respawn() 
    {
        GameObject player = gameObject;
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;        
    }
    //Respawn the player at center of the world. Called on client
    [ClientRPC]
    private void RespawnClient()
    {
        GameObject player = gameObject;
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
