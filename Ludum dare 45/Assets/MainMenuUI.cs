using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private List<LevelBoxUI> _levelBoxes;

    private void Start()
    {
        for (int i = 0; i < LevelManager.Instance.Levels.Count; i++)
        {
            _levelBoxes[i].gameObject.SetActive(true);
            _levelBoxes[i].Setup(LevelManager.Instance.Levels[i]);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneChanger.Instance.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
