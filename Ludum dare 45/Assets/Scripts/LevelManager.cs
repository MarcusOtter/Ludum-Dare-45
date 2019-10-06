using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    internal static LevelManager Instance { get; private set; }

    internal event PauseState OnPauseChanged;
    // Add reason for pausing in enum (failed, succeeded, manual, etc)
    internal delegate void PauseState(bool paused);

    [SerializeField] private Level[] _levels;

    private Level _currentLevel;
    private int _currentSheepKillCount;
    private bool _isPaused;

    private PlayerInput _playerInput;

    private void Awake()
    {
        SingletonCheck();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Initialize;
    }

    private void Initialize(Scene scene, LoadSceneMode mode)
    {
        if (_playerInput != null) { _playerInput.OnPauseKeyPressed -= HandlePauseKeyPressed; }

        _playerInput = FindObjectOfType<PlayerInput>();
        _playerInput.OnPauseKeyPressed += HandlePauseKeyPressed;
    }

    internal void PauseGame(bool pause)
    {
        OnPauseChanged?.Invoke(pause);
    }

    private void HandlePauseKeyPressed(object sender, EventArgs e)
    {
        _isPaused = !_isPaused;
        PauseGame(_isPaused);
    }

    internal void RegisterSheepKill()
    {
        if (++_currentSheepKillCount > _currentLevel.MaxSheepKilled)
        {

        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Initialize;
    }

    private void SingletonCheck()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
