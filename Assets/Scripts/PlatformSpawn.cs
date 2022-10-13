using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] m_ColisPathToSpawn;
    [SerializeField] private GameObject[] m_PiegePathToSpawn;
    [SerializeField] private GameObject[] m_MaisonPathToSpawn;
    [SerializeField] private GameObject[] m_TransitionPathToSpawn;
    [SerializeField] private GameObject m_FirstGameObjectsToSpawn;
    [SerializeField] private Transform m_PlayerTransform;

    private List<GameObject> m_GameObjectsSpawned = new List<GameObject>();

    private Vector3 m_StartPosition = new Vector3(0, 0, 0);
    private Vector3 m_OffSetVector = new Vector3(0, 0, 30);
    private Vector3 m_CurrentPosition;
    private Vector3 m_MiddlePosition;

    private GameObject GetRandomColisPathToSpawn()
    {
        int gameObjectsIndex = UnityEngine.Random.Range(0, this.m_ColisPathToSpawn.Length);
        return Instantiate(this.m_ColisPathToSpawn[gameObjectsIndex]);
    }

    private GameObject GetRandomPiegePathToSpawn()
    {
        int gameObjectsIndex = UnityEngine.Random.Range(0, this.m_PiegePathToSpawn.Length);
        return Instantiate(this.m_PiegePathToSpawn[gameObjectsIndex]);
    }

    private GameObject GetRandomMaisonPathToSpawn()
    {
        int gameObjectsIndex = UnityEngine.Random.Range(0, this.m_MaisonPathToSpawn.Length);
        return Instantiate(this.m_MaisonPathToSpawn[gameObjectsIndex]);
    }

    private GameObject GetRandomTransitionPathToSpawn()
    {
        int gameObjectsIndex = UnityEngine.Random.Range(0, this.m_TransitionPathToSpawn.Length);
        return Instantiate(this.m_TransitionPathToSpawn[gameObjectsIndex]);
    }

    private void RandomSpawn()
    {
        GameObject colisPathToSpawn = GetRandomColisPathToSpawn();
        GameObject piegePathToSpawn = GetRandomColisPathToSpawn();
        GameObject housePathToSpawn = GetRandomColisPathToSpawn();
        GameObject transitionPathToSpawn = GetRandomColisPathToSpawn();

        bool spawnBeforePiege = UnityEngine.Random.Range(0, 2) == 1;

        Spawn(colisPathToSpawn);

        if (spawnBeforePiege)
        {
            Spawn(transitionPathToSpawn);
        }

        Spawn(piegePathToSpawn);

        if (!spawnBeforePiege)
        {
            Spawn(transitionPathToSpawn);
        }

        Spawn(housePathToSpawn);
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
        this.RandomSpawn();
    }

    private void FixedUpdate()
    {
        Debug.Log("Distance between player and middle position : " + Vector3.Distance(m_PlayerTransform.position, m_MiddlePosition));
        if (this.m_PlayerTransform.position.z >= this.m_MiddlePosition.z)
        {
            for (int i = 0; i < 4; i++)
            {
                Destroy(this.m_GameObjectsSpawned[0]);
                this.m_GameObjectsSpawned.RemoveAt(0);
            }
            RandomSpawn();
        }
        Debug.Log("m_MiddlePosition Position : " + this.m_MiddlePosition.ToString());
    }
    #endregion
}
