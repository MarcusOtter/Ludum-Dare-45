using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class Level : ScriptableObject
{
    public int LevelNumber;
    public int SceneIndex;
    public int MaxSheepKilled;
}
