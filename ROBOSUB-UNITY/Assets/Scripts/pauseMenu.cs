using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class pauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public static float GameSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (GameIsPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Resume(){
        PauseMenuUI.SetActive(false);
        Time.timeScale = GameSpeed;
        GameIsPaused = false;
    }
    void Pause(){
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void LoadPool(){
        Resume();
        SceneManager.LoadScene("Pool");
    }
    public void LoadPond(){
        Resume();
        SceneManager.LoadScene("Pond");
    }

    public void GameSpeedGet(string gameSpeed){
        float GS = float.Parse(gameSpeed);
        GameSpeed = GS;
    }
}
