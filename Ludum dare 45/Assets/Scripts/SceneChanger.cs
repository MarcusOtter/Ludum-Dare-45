using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class SceneChanger : MonoBehaviour
{
    internal static SceneChanger Instance { get; private set; }

    [SerializeField] private string _leavingSceneParameterName = "LeavingScene";
    [SerializeField] private string _enteringSceneParameterName = "EnteringScene";
    [SerializeField] private float _waitTime = 0.5f;

    private Animator _animator;

    private int _leavingSceneAnimation;
    private int _enteringSceneAnimation;

    private IEnumerator _activeLoadingSequence;

    private void Awake()
    {
        SingletonCheck();

        _animator = GetComponent<Animator>();

        _leavingSceneAnimation = Animator.StringToHash(_leavingSceneParameterName);
        _enteringSceneAnimation = Animator.StringToHash(_enteringSceneParameterName);
    }

    internal void LoadNextScene()
    {
        ChangeSceneTo(SceneManager.GetActiveScene().buildIndex + 1);
    }

    internal void ReloadScene()
    {
        ChangeSceneTo(SceneManager.GetActiveScene().buildIndex);
    }

    private void ChangeSceneTo(int buildIndex)
    {
        if (_activeLoadingSequence != null) { return; }

        _activeLoadingSequence = LoadScene(buildIndex);
        StartCoroutine(_activeLoadingSequence);
    }

    private IEnumerator LoadScene(int buildIndex)
    {
        _animator.SetTrigger(_leavingSceneAnimation);

        yield return new WaitForSeconds(_waitTime);
        SceneManager.LoadScene(buildIndex);
        yield return new WaitForSeconds(_waitTime);

        _animator.SetTrigger(_enteringSceneAnimation);
        _activeLoadingSequence = null;
    }

    private void SingletonCheck()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
