using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{

    public AudioMixer audioMixer;
    AudioSource audioSource;
    [SerializeField] AudioClip Title;
    [SerializeField] AudioClip Level;
    [SerializeField] AudioClip GameOver;
    [SerializeField] AudioClip StageClear;

    [Header("Button")]
    public Button playButton;
    public Button settingsButton;
    public Button backButton;
    public Button quitButton;
    public Button resumeButton;
    public Button returnToMenuButton;
    public Button playAgainButton;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    [Header("Text")]
    public TMP_Text masterVolSliderText;
    public TMP_Text musicVolSliderText;
    public TMP_Text sfxVolSliderText;
    public TMP_Text livesText;
    public TMP_Text scoreText;

    [Header("Slider")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (resumeButton)
        {
            resumeButton.onClick.AddListener(Resume);
        }

        if (playButton)
        {
            playButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(1));
            audioSource.clip = Title;
            audioSource.Stop();
            audioSource.clip = Level; // Set the audio clip
            audioSource.Play();
        }

            

        if (settingsButton)
            settingsButton.onClick.AddListener(() => SetMenus(settingsMenu, mainMenu));

        if (backButton)
            backButton.onClick.AddListener(() => SetMenus(mainMenu, settingsMenu));

        if (quitButton)
            quitButton.onClick.AddListener(Quit);

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(RetToMenu);

        if (playAgainButton)
        {
            playAgainButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(1));
            audioSource.clip = Level; // Set the audio clip
            audioSource.Play();
        }


        if (masterVolSlider)
        {
            masterVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, masterVolSliderText, "MasterVol"));
            float mixerValue;
            audioMixer.GetFloat("MasterVol", out mixerValue);
            masterVolSlider.value = mixerValue + 80;
            if (masterVolSliderText)
                masterVolSliderText.text = masterVolSlider.value.ToString();
        }

        if (musicVolSlider)
        {
            musicVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, musicVolSliderText, "MusicVol"));
            float mixerValue;
            audioMixer.GetFloat("MusicVol", out mixerValue);
            musicVolSlider.value = mixerValue + 80;
            if (musicVolSliderText)
                musicVolSliderText.text = musicVolSlider.value.ToString();
        }

        if (sfxVolSlider)
        {
            sfxVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, sfxVolSliderText, "SFXVol"));
            float mixerValue;
            audioMixer.GetFloat("SFXVol", out mixerValue);
            sfxVolSlider.value = mixerValue + 80;
            if (sfxVolSliderText)
                sfxVolSliderText.text = sfxVolSlider.value.ToString();
        }

        if (livesText)
        {
            GameManager.Instance.OnLifeValueChanged.AddListener(UpdateLifeText);
            livesText.text = "Lives: " + GameManager.Instance.lives.ToString();
        }

        if (scoreText)
        {
            GameManager.Instance.OnScoreValueChanged.AddListener(UpdateScoreText);
            scoreText.text = "Score: " + GameManager.Instance.score.ToString();
        }

    }

    void SetMenus(GameObject menuToActivate, GameObject menuToDeactivate)
    {
        if (menuToActivate)
            menuToActivate.SetActive(true);

        if (menuToDeactivate)
            menuToDeactivate.SetActive(false);
    }

    void OnSliderValueChanged(float value, TMP_Text volSliderText, string paramName)
    {
        volSliderText.text = value.ToString();
        audioMixer.SetFloat(paramName, value - 80);
    }

    void UpdateLifeText(int value)
    {
        livesText.text = "Lives: " + value.ToString();
    }

    void UpdateScoreText(int value)
    {
        scoreText.text = "Score: " + value.ToString();
    }



    private void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }



    public static bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            //pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (!pauseMenu.activeSelf)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }


    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    void RetToMenu()
    {
        GameManager.Instance.ChangeScene(0);
    }




}