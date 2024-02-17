using System;
using UnityEngine;

public class GameState : MonoBehaviour, IInitializable
{
    public static GameState Instance;

    public Action GameStarted;
    public Action GamePaused;
    public Action GameUnpaused;
    public Action GameFinished;

    public Action ScoreAdded;

    public Action DifficultyChanged;

    public enum State
    { 
        InGame,
        Paused,
        Finished
    }
    public State CurrentState { get; private set; }

    public int Difficulty { get; private set; }

    [SerializeField] private int _scoreToChangeDifficult;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void OnEnable()
    {
        ScoreAdded += CheckDifficulty;
    }
    private void OnDisable()
    {
        ScoreAdded -= CheckDifficulty;
    }
    public void Initialize()
    {
        
    }
    public void ChangeState(State state)
    {
        switch (state)
        { 
            case State.InGame:
                    {
                        break;
                    }
            case State.Paused:
                {
                    break;
                }
            case State.Finished:
                {
                    break;
                }
        }
    }
    public void StartGame()
    {
        Difficulty = 1;

        GameStarted?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
    }
    public void PauseGame()
    {
        GamePaused?.Invoke();
        CurrentState = State.Paused;
        Time.timeScale = 0.0f;
    }
    public void UnpauseGame()
    {
        GameUnpaused?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
    }
    public void FinishGame()
    {
        GameFinished?.Invoke();
        CurrentState = State.Finished;

        PlayerPrefs.SetInt("LevelLocked" + (LevelManager.Instance.CurrentLevelId + 1), 0);

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Win, 1f);

        //if(AudioVibrationManager.Instance.IsVibrationEnabled)
            //Handheld.Vibrate();
    }
    public void CheckDifficulty()
    {
        if (PlayerScore.Instance.Score >= _scoreToChangeDifficult * Difficulty)
            ChangeDifficulty();
    }
    public void ChangeDifficulty()
    {
        Difficulty++;
        DifficultyChanged?.Invoke();
    }
}