using System;
using System.IO;
using UnityEngine;

/**
 * <summary>Serializable class for save the game </summary>
 */
public class SaveData 
{
    private SerializableGame m_SerializableGame;

    public int Score { get => this.m_SerializableGame.score; set => this.m_SerializableGame.score = value; }

    public int BestScore { get => this.m_SerializableGame.bestScore; set => this.m_SerializableGame.bestScore = value; }

    /**
     * <summary>The default constructor</summary> 
     */
    public SaveData() : this(0)
    {
    }

    /// <summary>
    /// Save data constructor
    /// </summary>
    /// <param name="score">The score</param>
    public SaveData(int score) : this(score, 0)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="score">The score</param>
    /// <param name="bestScore">The best score</param>
    public SaveData(int score, int bestScore) : this(new SerializableGame(score, bestScore))
    {
    }

    /// <summary>
    /// Save data constructor with <see cref="SerializableGame"/>
    /// </summary>
    /// <param name="serializableGame">The serializableGame</param>
    public SaveData(SerializableGame serializableGame)
    {
        this.m_SerializableGame = new SerializableGame(serializableGame);
    }

    /**
     * <summary>Save the game</summary>
     * <param name="save">The save</param>
     */
    public static void Save(SaveData save)
    {

        SaveData data = LoadPlayerRefs();

        if (save.BestScore >= data.BestScore)
        {
            data.BestScore = save.BestScore;
        }

        data.Score = save.Score;

        SaveData.SaveOnPlayerRef(data);
    }

    /**
     * <summary>Save data on Players refs</summary> 
     * 
     * <param name="saveGame">The save game</param>
     */
    public static void SaveOnPlayerRef(SaveData saveGame)
    {
        PlayerPrefs.SetInt("score", saveGame.Score);
        PlayerPrefs.SetInt("bestScore", saveGame.BestScore);
        Debug.Log("Save");
        PlayerPrefs.Save();
        Debug.Log(JsonUtility.ToJson(saveGame.m_SerializableGame));
    }

    /**
     * <summary>Load the save</summary>
     */
    public static SaveData LoadPlayerRefs()
    {
        int loadedGameScore = PlayerPrefs.GetInt("score");
        int loadedGameBestScore = PlayerPrefs.GetInt("bestScore");
        
        SaveData loadedSave = new SaveData(loadedGameScore, loadedGameBestScore);
        Debug.Log("Load");
        Debug.Log(JsonUtility.ToJson(loadedSave.m_SerializableGame));

        return loadedSave;
    }
}
