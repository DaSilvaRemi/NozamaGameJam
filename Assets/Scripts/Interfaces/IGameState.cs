using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IGameState
{
    /**
     * <summary>The game state</summary> 
     */
    public Tools.GameState GameState { get; set; }
}
