using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// The instance
    /// </summary>
    public static T Instance { get; private set; }

    /// <summary>
    /// Init and destroy the manager
    /// </summary>
    protected void InitManager()
    {
        if (!Manager<T>.Instance)
        {
            Manager<T>.Instance = this as T;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
