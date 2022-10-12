using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SerializableGame
{
    public int score;

    public int bestScore;

    /// <summary>
    /// SerializableGame copy constructor
    /// </summary>
    /// <param name="serializableGame">The serializableGame to copy</param>
    public SerializableGame(SerializableGame serializableGame): this(serializableGame.score, serializableGame.bestScore)
    {
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="time">The time</param>
    /// <param name="level">The level</param>
    /// <param name="bestTime">The best time</param>
    /// <param name="gameState">The game state</param>
    public SerializableGame(int score, int bestScore)
    {
        this.score = score;
        this.bestScore = bestScore;
    }

    /// <summary>
    /// Convert to string the current object
    /// </summary>
    /// <returns>The string form the object</returns>
    public override string ToString()
    {
        return $"Game Time : Score : {this.score}, Best Score : {this.bestScore}";
    }
}
