﻿using Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    FMOD.Studio.Bus Master;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                //Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    private void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        //Debug.Log(Time.timeScale);
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        //Debug.Log(Time.timeScale);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        Master.setVolume(volume);
    }

    public void ReturntoMenu()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("BacktoMenu");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
