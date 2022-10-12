using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : ObjectWillEarnThings
{
    #region MonoBehaviour methods
    /// <summary>
    /// OnCollisionEnter we check if the collision.gameObject is the player or an ThrowableObject, if it is we destroy this.gameObject and call <see cref="OnInteractionWithTheObjectEarnScore"/>
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject != null && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("ThrowableObject")))
        {
            base.OnInteractionWithTheObjectEarnScore(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
