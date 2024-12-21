using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Audio;

[DefaultExecutionOrder(-1)]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => instance;
    static GameManager instance;

    [SerializeField] PlayerController playerPrefab;
    public PlayerController PlayerInstance => playerInstance;
    PlayerController playerInstance = null;
    Transform currentCheckpoint;

    public UnityEvent<int> OnLifeValueChanged;
    public UnityEvent<int> OnScoreValueChanged;
    
    AudioSource myAudioSource;
    [SerializeField] AudioClip playerDeath;



    [SerializeField] int maxLives = 3;

    private int _lives;
    public int lives
    {
        get => _lives;
        set
        {
            if (_lives > value)
                Respawn();


            _lives = value;

            if (_lives > maxLives)
                _lives = maxLives;

            if (_lives < 1)
                GameOver();


            OnLifeValueChanged?.Invoke(_lives);
        }
    }

    
    private int _score = 0;
    public int score
    {
        get => _score;
        set
        {
            //if (_score < value)
            //invalid setting so throw error = possibly return out of function before setting varible

            _score = value;

            //if (TestMode) Debug.Log("Score has been set to: " + _score.ToString());
            OnScoreValueChanged?.Invoke(_score);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        _lives = maxLives;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int buildIndex = (SceneManager.GetActiveScene().name == "Level") ? 0 : 1; //Turnery statement. 0 is True and 1 is False. Cleaner than an If/Else statement
            SceneManager.LoadScene(buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Level")
                SceneManager.LoadScene(0);
            if (SceneManager.GetActiveScene().name == "Title")
                SceneManager.LoadScene(1);
            if (SceneManager.GetActiveScene().name == "GameOver")
                SceneManager.LoadScene(0);
        }

    }


    // This is for the Play Button
    public void ChangeScene(int buildIndex)
    {
        SceneManager.LoadScene(1);
    }


    public void SpawnPlayer(Transform spawnLocation)
    {
        playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        currentCheckpoint = spawnLocation;

    }

    public void UpdateCheckpoint(Transform updatedCheckpoint)
    {
        currentCheckpoint = updatedCheckpoint;
    }


    void Respawn()
    {
        Debug.Log("Respawn Called");
        playerInstance.transform.position = currentCheckpoint.position;

        Debug.Log("Lives:" + GameManager.Instance._lives);
    }


    void GameOver()
    {
        Debug.Log("Game Over Called");
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(playerDeath);
        SceneManager.LoadScene(2);
        _lives = maxLives;
        _score = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("GameComplete"))
        {
            SceneManager.LoadScene("GameOver");
            _score = 0;
        }


    }


}
