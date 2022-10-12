using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITime
{
    /**
     * <summary>The time</summary> 
     */
    float Time { get; }

    /**
     * <summary>The best time</summary> 
     */
    float BestTime { get; }
}