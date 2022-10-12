using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnScore : MonoBehaviour, IScore
{
    [Header("The score to earn")]
    [SerializeField] private int m_Score;

    /// <summary>
    /// The score
    /// </summary>
    public int Score { get => this.m_Score; }
}
