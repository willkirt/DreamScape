using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Rendering;

public class GameManager : Singleton<GameManager>
{
    [Header("Prefabs")]
    [SerializeField] GameObject projectionPrefab;
    [SerializeField] GameObject batPrefab;

    [Header("GameObjects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject projection;
    [SerializeField] GameObject sceneCamera;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject levelManager;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject scoreText;
    [SerializeField] GameObject textBox;

    [Header("Materials")]
    [Tooltip("Material for the nightmare state skybox.")]
    [SerializeField] Material nightmare;
    [Tooltip("Material for the dream state skybox.")]
    [SerializeField] Material dream;

    [Header("Gamestates and variables.")]
    [SerializeField] GameStates currentState;
    [SerializeField] bool isPortalActive = false;
    [SerializeField] bool reset = false;
    [SerializeField] int finalScore;
    [SerializeField] int levelScore;

    public enum GameStates { Menu, Play, Pause }
    public GameStates CurrentState { get => currentState; set => currentState = value; }
    public int LevelScore { get => levelScore; set => levelScore = value; }
    public int FinalScore { get => finalScore; set => finalScore = value; }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += LevelWasLoaded;
    }

    public void LevelWasLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + " was loaded.");
        //Attempts to find key game objects in the new scene. send an error message if object is not found.
        #region GameObject assignment
        try{ player = GameObject.Find("Player"); }
        catch { Debug.Log("Player Object not found."); }

        try { sceneCamera = GameObject.Find("Main Camera"); }
        catch { Debug.Log("Camera not found."); }

        try { levelManager = GameObject.Find("LevelManager"); }
        catch { Debug.Log("LevelManager not found."); }

        try { pauseMenu = GameObject.Find("PauseMenu"); }
        catch { Debug.Log("GameUI not found."); }

        try { scoreText = GameObject.Find("ScoreText"); }
        catch { Debug.Log("score text not found."); }

        try { textBox = GameObject.Find("GameTextBox"); }
        catch { Debug.Log("Textbox not found."); }
        #endregion

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        if (textBox != null)
        {
            if (!reset)
            {
                textBox.SetActive(false);
            }
            else
            {
                StartCoroutine(PopupText("Not again."));
            }
        }
                
        if ( scene.name == "GameOver" && player.GetComponent<CharacterScript>().HasWon == false)
        {
            player.GetComponent<CharacterScript>().HasWon = true;
        }

        if (currentState == GameStates.Menu)
        {
            SoundManager.Instance.musicSource2.Stop();
            SoundManager.Instance.PlayMusic("MenuMusic", SoundManager.Instance.musicSource);
        }
        else if (currentState == GameStates.Play)
        {
            SoundManager.Instance.musicSource2.Stop();
            SoundManager.Instance.PlayMusic("Dream1", SoundManager.Instance.musicSource);
        }
        levelScore = 0;
        UpdateUI();

        reset = false;
    }

    public void ActivateProjection()
    {
        // Disable the player model
        player.GetComponentInChildren<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterScript>().IsProjection = true;

        // Instantiate Projection and set location to player
        projection = Instantiate(projectionPrefab, player.transform.position, player.transform.rotation);

        // Change target of enemies currently chasing
        levelManager.GetComponent<NPCManager>().UpdateTarget(projection);

        // Switch main camera to projection
        sceneCamera.GetComponent<CameraScript>().cm.LookAt = projection.transform;
        sceneCamera.GetComponent<CameraScript>().cm.Follow = projection.transform;
        sceneCamera.GetComponent<CameraScript>().player = projection.transform;
        sceneCamera.GetComponent<CameraScript>().orientation = GameObject.Find("ProjDir").transform;
    }

    public void DeActivateProjection()
    {
        // Enable the player model
        player.GetComponentInChildren<CapsuleCollider>().enabled = true;
        player.GetComponent<CharacterScript>().IsProjection = false;

        // Destroy Projection
        Destroy(projection);

        // Change target of enemies currently chasing
        levelManager.GetComponent<NPCManager>().UpdateTarget(player);

        // Switch main camera to player
        sceneCamera.GetComponent<CameraScript>().cm.LookAt = player.transform;
        sceneCamera.GetComponent<CameraScript>().cm.Follow = player.transform;
        sceneCamera.GetComponent<CameraScript>().player = player.transform;
        sceneCamera.GetComponent<CameraScript>().orientation = GameObject.Find("PlayerOrientation").transform;
    }

    public void ActivateEnemy()
    {
        if (levelManager != null)
        {
            levelManager.GetComponent<NPCManager>().SpawnEnemy("Bat", 0);
            levelManager.GetComponent<NPCManager>().SpawnEnemy("PatrolBat", 2);
            levelManager.GetComponent<NPCManager>().SpawnEnemy("PatrolBat", 4);
            ChangeSkyBox();
            SoundManager.Instance.PlayMusic("Nightmare2", SoundManager.Instance.musicSource2);
            SoundManager.Instance.PlayMusic("Nightmare1", SoundManager.Instance.musicSource);
        }
    }

    public void SceneReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        levelScore = 0;
        SoundManager.Instance.musicSource2.Stop();
        SoundManager.Instance.PlayMusic("Dream1", SoundManager.Instance.musicSource);
        reset = true;
    }

    public void DropTarget()
    {
        levelManager.GetComponent<NPCManager>().StopChasing();
    }

    public void MainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void ChangeSkyBox()
    {
        if (RenderSettings.skybox == dream)
        {
            RenderSettings.skybox = nightmare;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(28 / 255f, 12 / 255f, 103 / 255f);
            RenderSettings.ambientEquatorColor = new Color(33 / 255f, 56 / 255f, 103 / 255f);
            RenderSettings.ambientGroundColor = new Color(0, 0, 0);
            RenderSettings.sun.intensity = .1f;
            RenderSettings.fog = true;
        }
        else
        {
            RenderSettings.skybox = dream;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            RenderSettings.sun.intensity = 1;
            RenderSettings.fog = false;
        }
    }

    public void ChangeState(string newState)
    {
        switch (newState)
        {
            case "Menu":
                currentState = GameStates.Menu;
                Time.timeScale = 1;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                finalScore = 0;
                UpdateUI();
                break;
            case "Play":
                currentState = GameStates.Play;
                Time.timeScale = 1;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if (pauseMenu != null)
                {
                    pauseMenu.SetActive(false);
                }
                UpdateUI();
                break;
            case "Pause":
                currentState = GameStates.Pause;
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                pauseMenu.SetActive(true);
                break;
            default:
                currentState = GameStates.Menu;
                Time.timeScale = 1;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                finalScore = 0;
                break;
        }
    }

    public void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + (finalScore + levelScore).ToString();
        }
    }

    public void DisplayText(string text)
    {
        StartCoroutine(PopupText(text));
    }

    public IEnumerator PopupText(string text)
    {
        textBox.GetComponentInChildren<TextMeshProUGUI>().text = text;
        textBox.SetActive(true);
        yield return new WaitForSeconds(5);
        textBox.SetActive(false);
    }
}
