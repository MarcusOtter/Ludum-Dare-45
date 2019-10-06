using UnityEngine;

public class UIManager : MonoBehaviour
{
    internal static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _pauseMenu;

    private void Awake()
    {
        SingletonCheck();
    }

    private void Start()
    {
        LevelManager.Instance.OnPauseChanged += HandlePauseChanged;
    }

    private void HandlePauseChanged(bool paused)
    {
        _pauseMenu.SetActive(paused);
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
