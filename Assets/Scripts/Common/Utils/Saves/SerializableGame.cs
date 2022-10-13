using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SerializableGame
{
    public int nbColisLivree;

    public int bestNbColisLivree;

    public int nbColisNonLivree;

    public int bestColisNonLivree;

    /// <summary>
    /// SerializableGame copy constructor
    /// </summary>
    /// <param name="serializableGame">The serializableGame to copy</param>
    public SerializableGame(SerializableGame serializableGame): this(serializableGame.nbColisLivree, serializableGame.bestNbColisLivree, serializableGame.nbColisNonLivree, serializableGame.bestColisNonLivree)
    {
    }


    public SerializableGame(int colisLivree, int bestColisLivree, int nbColisNonLivree, int bestColisNonLivree)
    {
        this.nbColisLivree = colisLivree;
        this.bestNbColisLivree = bestColisLivree;
        this.nbColisNonLivree = bestColisLivree;
        this.bestColisNonLivree = bestColisLivree;
    }

    /// <summary>
    /// Convert to string the current object
    /// </summary>
    /// <returns>The string form the object</returns>
    public override string ToString()
    {
        return $"Game NbColisLivree : {this.nbColisLivree}, BestNbColisLivree : {this.bestNbColisLivree}, NbColisNonLivree : {this.nbColisNonLivree}, BestNbColisNonLivree : {this.bestColisNonLivree}";
    }
}
