using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class Level : ScriptableObject
{   
    // For resetting purposes, variables are accessible in editor.
    public bool IsCompleted;
    public int SavePercent;
    public int MostAmountOfSavedSheep;
    public int LevelNumber;
    public int SceneIndex;
    public int MaxSheepKilled;

    internal void Complete(int savedSheep, int killedSheep)
    {
        var newSavePercent = (int) (savedSheep / (float) (savedSheep + killedSheep) * 100);

        if (newSavePercent > SavePercent) { SavePercent = newSavePercent; }
        if (savedSheep > MostAmountOfSavedSheep) { MostAmountOfSavedSheep = savedSheep; }

        IsCompleted = true;
    }
}
