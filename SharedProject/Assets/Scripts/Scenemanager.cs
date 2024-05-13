using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.Register(GameEventTypes.ReadyToStart, OnReady);
    }

    private void OnReady(object sender, EventArgs e)
    {
        SceneManager.LoadScene("SampleScene");
        Debug.Log("Carga game");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void StartMenu()
    {
        /*if (PlayerSpawner.Instance.currentPlayers == 2)
        {
            EventManager.Instance.Dispatch(GameEventTypes.ReadyToStart, this, EventArgs.Empty);
        }
        else Debug.Log("No hay 2 players");*/
    }
    public void ExitGame()
    {
        ExitGame();
    }

}
