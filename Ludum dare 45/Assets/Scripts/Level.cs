using UnityEngine;

[CreateAssetMenu(menuName = "New level")]
public class Level : ScriptableObject
{
    public int LevelNumber;
    public int SceneIndex;
    public int MaxSheepKilled;
}
