﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 1f;
    public GameOverScreen GameOverScreen;
	int maxPlatform =0;
    // Start is called before the first frame update
    public void EndGame()
    {
        if(gameHasEnded == false){
            gameHasEnded = true;
            Debug.Log("GAME OVER");
            GameOverScreen.Setup();
            //Invoke("Restart",restartDelay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
