using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectCanBeTriggerred : ObjectWillEarnThings
{
    void OnObjectTriggered(GameObject gameObject)
    {
        base.OnInteractionWithTheObjectEarnScore(gameObject);
    }

    /// <summary>
    /// OnTriggerEnter we call <see cref="OnObjectTriggered"/>
    /// </summary>
    /// <param name="other">The collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            this.OnObjectTriggered(other.gameObject);
        }
    }
}
