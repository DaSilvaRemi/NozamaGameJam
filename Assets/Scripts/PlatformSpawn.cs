using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] m_GameObjectsToSpawn;
    [SerializeField] private GameObject m_FirstGameObjectsToSpawn;
    [SerializeField] private Transform m_PlayerTransform;

    private List<GameObject> m_GameObjectsSpawned = new List<GameObject>();

    private Vector3 m_StartPosition = new Vector3(0, 0, 0);
    private Vector3 m_OffSetVector = new Vector3(0, 0, 30);
    private Vector3 m_CurrentPosition;
    private Vector3 m_MiddlePosition;

    private GameObject GetRandomObjectToSpawn()
    {
        int gameObjectsIndex = UnityEngine.Random.Range(0, this.m_GameObjectsToSpawn.Length);
        return Instantiate(this.m_GameObjectsToSpawn[gameObjectsIndex]);
    }

    private void Spawn()
    {
        GameObject go = GetRandomObjectToSpawn();
        Spawn(go);
    }

    private void Spawn(GameObject go)
    {
        go.transform.SetPositionAndRotation(this.m_CurrentPosition, Quaternion.identity);
        this.m_MiddlePosition = this.m_CurrentPosition + m_OffSetVector / 2;
        this.m_CurrentPosition += this.m_OffSetVector;
        this.m_GameObjectsSpawned.Add(go);
    }

    #region MonoBehaviour Methods
    private void Awake()
    {
        this.m_CurrentPosition = m_StartPosition;
        this.Spawn(Instantiate(this.m_FirstGameObjectsToSpawn));
        this.Spawn();
    }

    private void FixedUpdate()
    {
        Debug.Log("Distance between player and middle position : " + Vector3.Distance(m_PlayerTransform.position, m_MiddlePosition));
        if (this.m_PlayerTransform.position.z >= this.m_MiddlePosition.z)
        {
            Destroy(this.m_GameObjectsSpawned[0]);
            this.m_GameObjectsSpawned.RemoveAt(0);
            Spawn();
            //this.m_MiddlePosition = this.m_GameObjectsSpawned[0].transform.position + this.m_OffSetVector;
        }
        Debug.Log("m_MiddlePosition Position : " + this.m_MiddlePosition.ToString());
    }
    #endregion
}
