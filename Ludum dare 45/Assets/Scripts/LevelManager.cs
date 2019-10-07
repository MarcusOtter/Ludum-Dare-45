using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    internal static LevelManager Instance { get; private set; }

    internal Level CurrentLevel => Levels[_currentLevelIndex];
    internal Level AvailableLevel => Levels[Levels.IndexOf(Levels.LastOrDefault(x => x.IsCompleted)) + 1];

    internal event PauseDelegate OnPauseChanged;
    internal delegate void PauseDelegate(PauseState state);

    internal List<Sheep> ActiveSheep { get; private set; } = new List<Sheep>();
    internal List<Sheep> SafeSheep { get; private set; } = new List<Sheep>();
    internal List<Sheep> DeadSheep { get; private set; } = new List<Sheep>();

    [Header("References")]
    [SerializeField] private GameObject _fakeSheepPrefab;
    [SerializeField] internal List<Level> Levels = new List<Level>();

    [Header("Sheep pen bounds (top view)")]
    [SerializeField] private Vector3 _sheepPenBottomLeft = new Vector3(-11.2f, 0, -11.2f);
    [SerializeField] private Vector3 _sheepPenTopRight = new Vector3(6.4f, 0, -4.8f);

    private int _currentLevelIndex;

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
        UIManager.Instance.CheckClosePause();

        _currentLevelIndex = Levels.IndexOf(Levels.FirstOrDefault(x => x.SceneIndex == SceneManager.GetActiveScene().buildIndex));

        SpawnPreviousLevelSheep();

        ActiveSheep = FindObjectsOfType<Sheep>().ToList();
        DeadSheep = new List<Sheep>();
        SafeSheep = new List<Sheep>();

        if (_playerInput != null)
        {
            _playerInput.OnPauseKeyPressed -= HandlePauseKeyPressed;
            _playerInput.OnRestartKeyPressed -= HandleRestartKeyPressed;
        }

        _playerInput = FindObjectOfType<PlayerInput>();
        _playerInput.OnPauseKeyPressed += HandlePauseKeyPressed;
        _playerInput.OnRestartKeyPressed += HandleRestartKeyPressed;
    }

    private void SpawnPreviousLevelSheep()
    {
        int savedSheep = 0;
        for (int i = _currentLevelIndex - 1; i >= 0; i--)
        {
            Level level = Levels[i];
            savedSheep += level.MostAmountOfSavedSheep;
        }

        for(int i = 0; i < savedSheep; i++)
        {
            var positionX = Random.Range(_sheepPenBottomLeft.x, _sheepPenTopRight.x);
            var positionY = 0.1f;
            var positionZ = Random.Range(_sheepPenBottomLeft.z, _sheepPenTopRight.z);

            Instantiate(_fakeSheepPrefab, new Vector3(positionX, positionY, positionZ), Quaternion.identity);
        }
    }

    internal void LoadNextLevel()
    {
        _currentLevelIndex++;

        print("index " + _currentLevelIndex);
        print("scenes " + (SceneManager.sceneCountInBuildSettings - 1));
        if(_currentLevelIndex >= SceneManager.sceneCountInBuildSettings -1)
        {
            print("what");
            SceneChanger.Instance.LoadMenuScene();
            return;
        }

        SceneChanger.Instance.LoadScene(CurrentLevel.SceneIndex);
    }

    internal void RegisterLevelChange()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) { return; }
        SetPauseState(PauseState.NotPaused);
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

    internal void RegisterSavedSheep(Sheep sheep)
    {
        if (!ActiveSheep.Contains(sheep)) { return; }

        ActiveSheep.Remove(sheep);
        SafeSheep.Add(sheep);

        UIManager.Instance.UpdateUI();

        if (ActiveSheep.Count == 0)
        {
            CompleteCurrentLevel();
        }
    }

    internal void RegisterSheepDeath(Sheep sheep)
    {
        if (!ActiveSheep.Contains(sheep)) { return; }

        ActiveSheep.Remove(sheep);
        DeadSheep.Add(sheep);

        UIManager.Instance.UpdateUI();

        if (DeadSheep.Count > CurrentLevel.MaxSheepKilled)
        {
            SetPauseState(PauseState.LevelFailed);
            return;
        }

        if (ActiveSheep.Count == 0)
        {
            CompleteCurrentLevel();
        }
    }

    private void CompleteCurrentLevel()
    {
        CurrentLevel.Complete(SafeSheep.Count, DeadSheep.Count);
        SetPauseState(PauseState.LevelCompleted);
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
