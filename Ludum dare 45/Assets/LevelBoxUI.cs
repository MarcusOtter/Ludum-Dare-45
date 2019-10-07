using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelBoxUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private TextMeshProUGUI _percentCompletion;

    private Level _level;

    public void Setup(Level level)
    {
        _level = level;
        _levelNumber.text = _level.LevelNumber.ToString();
        _percentCompletion.text = _level.SavePercent.ToString() + "%";
        GetComponent<Button>().interactable = _level.IsCompleted || _level == LevelManager.Instance.AvailableLevel;
    }

    public void ChangeLevelToSelf()
    {
        SceneChanger.Instance.LoadScene(_level.SceneIndex);
    }
}
