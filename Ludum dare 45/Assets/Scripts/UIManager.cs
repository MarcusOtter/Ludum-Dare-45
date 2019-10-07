using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    internal static UIManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _nextLevelButton;
    [SerializeField] private TextMeshProUGUI _pauseMenuHeader;
    [SerializeField] private TextMeshProUGUI _levelHeader;
    [SerializeField] private TextMeshProUGUI _maxDeathsHeader;

    [SerializeField] private GameObject[] _activeSheep;
    [SerializeField] private GameObject[] _deadSheep;
    [SerializeField] private GameObject[] _safeSheep;

    private void Awake()
    {
        SingletonCheck();
    }

    private void Start()
    {
        LevelManager.Instance.OnPauseChanged += HandlePauseChanged;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0) { return; }
        UpdateUI();
    }

    internal void CheckClosePause()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            HandlePauseChanged(PauseState.NotPaused);
        }
    }

    // Called by button
    public void GoToMainMenu()
    {
        SceneChanger.Instance.LoadMenuScene();
    }

    // Called by button
    public void RestartLevel()
    {
        FindObjectOfType<PlayerInput>().ForceRestartEvent();
    }

    // Called by button
    public void LoadNextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }

    internal void UpdateUI()
    {
        var levelManager = LevelManager.Instance;
        _levelHeader.text = GetLevelHeader();
        _maxDeathsHeader.text = GetMaxDeathsHeader();

        _nextLevelButton.SetActive(levelManager.CurrentLevel.IsCompleted);

        for (int i = 0; i < _activeSheep.Length; i++)
        {
            _activeSheep[i].SetActive(false);
            _deadSheep[i].SetActive(false);
            _safeSheep[i].SetActive(false);
        }

        for (int i = 0; i < levelManager.ActiveSheep.Count; i++)
        {
            _activeSheep[i].SetActive(true);
        }

        for (int i = 0; i < levelManager.DeadSheep.Count; i++)
        {
            _deadSheep[i].SetActive(true);
        }

        for (int i = 0; i < levelManager.SafeSheep.Count; i++)
        {
            _safeSheep[i].SetActive(true);
        }
    }

    private string GetLevelHeader() => $"LEVEL: {LevelManager.Instance.CurrentLevel.LevelNumber}";
    private string GetMaxDeathsHeader() => $"MAX DEATHS: {LevelManager.Instance.CurrentLevel.MaxSheepKilled}";

    private string GetMenuHeader(PauseState state)
    {
        switch (state)
        {
            default:
            case PauseState.NotPaused:      return "Something went wrong..";

            case PauseState.LevelCompleted: return "SHEEP SAVED";
            case PauseState.LevelFailed:    return "GAME OVER";
            case PauseState.ManuallyPaused: return "PAUSED";
        }
    }

    private void HandlePauseChanged(PauseState state)
    {
        UpdateUI();
        _pauseMenuHeader.text = GetMenuHeader(state);
        _pauseMenu.SetActive(state != PauseState.NotPaused);
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnPauseChanged -= HandlePauseChanged;
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
