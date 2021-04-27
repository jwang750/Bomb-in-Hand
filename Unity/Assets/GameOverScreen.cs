using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
   // public Text pointsText;
    // Start is called before the first frame update
     public float restartDelay = 1f;

    public void Setup(){
        gameObject.SetActive(true);
    }

    public void RestartButton(){
       // SceneManager.LoadScene("Game");

            Invoke("Restart",restartDelay);
        
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
