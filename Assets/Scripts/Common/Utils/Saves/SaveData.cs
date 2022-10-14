using System;
using System.IO;
using UnityEngine;

/**
 * <summary>Serializable class for save the game </summary>
 */
public class SaveData 
{
    private SerializableGame m_SerializableGame;

    public int NbColisLivree { get => this.m_SerializableGame.nbColisLivree; set => this.m_SerializableGame.nbColisLivree = value; }

    public int BestNbColisLivree { get => this.m_SerializableGame.bestNbColisLivree; set => this.m_SerializableGame.bestNbColisLivree = value; }

    public int NbColisNonLivree { get => this.m_SerializableGame.nbColisNonLivree; set => this.m_SerializableGame.nbColisNonLivree = value; }
    public int BestNbColisNonLivree { get => this.m_SerializableGame.bestColisNonLivree; set => this.m_SerializableGame.bestColisNonLivree = value; }

    /**
     * <summary>The default constructor</summary> 
     */
    public SaveData() : this(0)
    {
    }

    /// <summary>
    /// Save data constructor
    /// </summary>
    /// <param name="nbColisLivree">The score</param>
    public SaveData(int nbColisLivree) : this(nbColisLivree, nbColisLivree)
    {
    }

    public SaveData(int nbColisLivree, int nbColisNonLivree) : this(nbColisLivree, nbColisLivree, nbColisNonLivree)
    {
    }

    public SaveData(int nbColisLivree, int bestNbColisLivree, int nbColisNonLivree) : this(nbColisLivree, bestNbColisLivree, nbColisNonLivree, nbColisNonLivree)
    {
    }

    public SaveData(int nbColisLivree, int bestNbColisLivree, int nbColisNonLivree, int bestNbColisNonLivree) : this(new SerializableGame(nbColisLivree, bestNbColisLivree, nbColisNonLivree, bestNbColisNonLivree))
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

        if (save.NbColisLivree >= data.BestNbColisLivree)
        {
            data.BestNbColisLivree = save.BestNbColisLivree;
        }

        if (save.NbColisNonLivree <= data.BestNbColisNonLivree)
        {
            data.BestNbColisNonLivree = save.NbColisNonLivree;
        }

        save.NbColisLivree = data.NbColisLivree;
        save.NbColisNonLivree = data.NbColisNonLivree;

        SaveData.SaveOnPlayerRef(data);
    }

    /**
     * <summary>Save data on Players refs</summary> 
     * 
     * <param name="saveGame">The save game</param>
     */
    public static void SaveOnPlayerRef(SaveData saveGame)
    {
        PlayerPrefs.SetInt("nbColisLivree", saveGame.NbColisLivree);
        PlayerPrefs.SetInt("bestNbColisLivree", saveGame.BestNbColisLivree);
        PlayerPrefs.SetInt("nbColisNonLivree", saveGame.NbColisNonLivree);
        PlayerPrefs.SetInt("bestNbColisNonLivree", saveGame.BestNbColisNonLivree);
        Debug.Log("Save");
        PlayerPrefs.Save();
        Debug.Log(JsonUtility.ToJson(saveGame.m_SerializableGame));
    }

    /**
     * <summary>Load the save</summary>
     */
    public static SaveData LoadPlayerRefs()
    {

        int loadedGameNbColis = PlayerPrefs.GetInt("nbColisLivree");
        int loadedGameBestNbColis = PlayerPrefs.GetInt("bestNbColisLivree");
        int loadedGameNbColisNonLivree = PlayerPrefs.GetInt("nbColisNonLivree");
        int loadedGameBestNbColisNonLivree = PlayerPrefs.GetInt("bestNbColisNonLivree");
        
        SaveData loadedSave = new SaveData(loadedGameNbColis, loadedGameBestNbColis, loadedGameNbColisNonLivree, loadedGameBestNbColisNonLivree);
        Debug.Log("Load");
        Debug.Log(JsonUtility.ToJson(loadedSave.m_SerializableGame));

        return loadedSave;
    }
}
