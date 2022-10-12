using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnStock : MonoBehaviour, IStock
{
    [Header("The stock to earn")]
    [SerializeField] private int m_Stock;

    /// <summary>
    /// The stock
    /// </summary>

    public int Stock { get => this.m_Stock; }
}
