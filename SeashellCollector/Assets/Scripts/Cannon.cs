using Assets.Scripts;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Cannon script has been initialized.");
    }

    public void ShootToPlayer(List<Pickup> pickups)
    {
        Debug.Log("Shoot to player");
        var player = FindFirstObjectByType<Player>();
        Debug.Log("TODO explostion animation at player of shells and pearls and coral.");
        player.AddPickups(pickups);
    }
}
