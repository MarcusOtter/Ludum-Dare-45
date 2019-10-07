using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    internal static LevelManager Instance { get; private set; }

    internal event PauseDelegate OnPauseChanged;
    internal delegate void PauseDelegate(PauseState state);

    [SerializeField] private Level[] _levels;

    private int _currentLevelIndex;
    private int _currentSheepKillCount;

    private PauseState _currentPauseState;

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
        if (_playerInput != null)
        {
            _playerInput.OnPauseKeyPressed -= HandlePauseKeyPressed;
            _playerInput.OnRestartKeyPressed -= HandleRestartKeyPressed;
        }

        _playerInput = FindObjectOfType<PlayerInput>();
        _playerInput.OnPauseKeyPressed += HandlePauseKeyPressed;
        _playerInput.OnRestartKeyPressed += HandleRestartKeyPressed;
    }

    private void HandleRestartKeyPressed(object sender, EventArgs e)
    {
        SetPauseState(PauseState.NotPaused);
    }

    private void SetPauseState(PauseState state)
    {
        _currentPauseState = state;
        OnPauseChanged?.Invoke(state);
    }

    private void HandlePauseKeyPressed(object sender, EventArgs e)
    {
        switch (_currentPauseState)
        {
            case PauseState.NotPaused:
                SetPauseState(PauseState.ManuallyPaused);
                break;

            case PauseState.ManuallyPaused:
                SetPauseState(PauseState.NotPaused);
                break;
        }
    }

    internal void RegisterPlayerDeath()
    {
        SetPauseState(PauseState.LevelFailed);
    }

    internal void RegisterSheepDeath()
    {
        _currentSheepKillCount++;

        if (_currentSheepKillCount > _levels[_currentLevelIndex].MaxSheepKilled)
        {
            SetPauseState(PauseState.LevelFailed);
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
