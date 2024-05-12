using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] GameObject PlayerPrefab;
    public int currentPlayers = 0;
    [SerializeField] Transform playerOneSpawner;
    [SerializeField] Transform playerTwoSpawner;
    public static PlayerSpawner Instance { get; private set; }

    private void Awake()
    {
        Instance = this;   
    }
    public void PlayerJoined(PlayerRef player)
    {
        if (currentPlayers < 2)
        {
            if (player == Runner.LocalPlayer)
            {
                foreach (var item in Runner.ActivePlayers)
                {
                    Debug.Log(currentPlayers);
                    currentPlayers++;
                    Debug.Log(currentPlayers);
                }
                if (currentPlayers == 1)
                {
                    Runner.Spawn(PlayerPrefab, playerOneSpawner.position, Quaternion.identity);
                }

                if (currentPlayers == 2)
                {
                    Runner.Spawn(PlayerPrefab, playerTwoSpawner.position, Quaternion.identity);
                }

            }
        }

    }
}
