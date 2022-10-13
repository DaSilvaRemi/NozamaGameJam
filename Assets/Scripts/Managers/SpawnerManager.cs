using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class SpawnerManager : MonoBehaviour, IEventHandler
{
    [Header("Spawner Manager GameObjects properties")]
    [Tooltip("List Game objects to spawn")]
    [SerializeField] private GameObject[] m_GameObjectsToSpawn;
    [Tooltip("Spawns position")]
    [SerializeField] private Transform[] m_SpawnerTransforms;

    [Header("Spawner Manager limit properties")]
    [Tooltip("Cooldown duration")]
    [SerializeField] private float m_SpawnCooldownDuration = 0.2f;
    [Tooltip("Spawn Limit")]
    [SerializeField] private float m_SpawnLimit = 30;
    [Tooltip("Is Spawning continously")]
    [SerializeField] private bool m_IsTimedSpawning = false;

    private List<GameObject> m_GameObjectsSpawned;
    private float m_NextSpawnTime;
    private IEnumerator m_TimedSpawnCoroutine;

    #region SpawnManager properties
    public bool IsTimedSpawnLimit { get => this.m_IsTimedSpawning; }

    public bool HasReachedSpawnLimit { get => this.m_GameObjectsSpawned.Count >= this.m_SpawnLimit; }
    #endregion

    #region SpawnerManager Spawn Methods
    /// <summary>
    /// Get a random object to spawn by spawner
    /// </summary>
    /// <returns>The GameObject to spawn</returns>
    private GameObject GetRandomObjectToSpawn()
    {
        int gameObjectsIndex = UnityEngine.Random.Range(0, this.m_GameObjectsToSpawn.Length);
        return Instantiate(this.m_GameObjectsToSpawn[gameObjectsIndex]);
    }

    /// <summary>
    /// Get the transform to spawn an object
    /// </summary>
    /// <returns>The transform to spawn</returns>
    private Transform GetRandomSpawnerTransform()
    {
        int spawnerTransformIndex = UnityEngine.Random.Range(0, this.m_SpawnerTransforms.Length);
        return this.m_SpawnerTransforms[spawnerTransformIndex];
    }

    /// <summary>
    /// Spawn an object in spawns
    /// </summary>
    private void Spawn()
    {
        this.Spawn(this.GetRandomObjectToSpawn());
    }

    /// <summary>
    /// Spawn a game object
    /// </summary>
    /// <param name="gameObjectToSpawn">Game objects to spawn</param>
    private void Spawn(GameObject gameObjectToSpawn)
    {
        this.Spawn(gameObjectToSpawn, GetRandomSpawnerTransform());
    }

    /// <summary>
    /// Spawn an game object with a the transform spawn
    /// </summary>
    /// <param name="gameObjectToSpawn">The gamoe object to spawn</param>
    /// <param name="spawnerTransform">The Spawn Transform</param>
    private void Spawn(GameObject gameObjectToSpawn, Transform spawnerTransform)
    {
        if (this.HasReachedSpawnLimit)
        {
            Destroy(gameObjectToSpawn);
            return;
        }

        gameObjectToSpawn.transform.SetPositionAndRotation(spawnerTransform.position, spawnerTransform.rotation);
        this.m_GameObjectsSpawned.Add(gameObjectToSpawn);
    }

    /// <summary>
    /// Spawn a number of objectds
    /// </summary>
    /// <param name="numberGoToSpawn">Number Go to spawn</param>
    private void Spawn(int numberGoToSpawn)
    {
        for (int i = 0; i < numberGoToSpawn; i++)
        {
            this.Spawn();
        }
    }

    /// <summary>
    /// Spawn a list of game objects
    /// </summary>
    /// <param name="gameObjectsToSpawn">List to spawn</param>
    private void Spawn(List<GameObject> gameObjectsToSpawn)
    {
        for (int i = 0; i < gameObjectsToSpawn.Count; i++)
        {
            this.Spawn(gameObjectsToSpawn[i]);
        }
    }

    /// <summary>
    /// Spawn a GameOject at each next cooldown
    /// </summary>
    private void SpawnEachNextCooldown()
    {
        if (this.m_IsTimedSpawning && Time.time > m_NextSpawnTime)
        {
            this.m_NextSpawnTime = this.m_SpawnCooldownDuration + Time.time;
            this.Spawn();
        }
    }

    /// <summary>
    /// Spawn each time a game object
    /// </summary>
    /// <param name="time">The time to spawn other game object</param>
    private void SpawnEachTime(float time)
    {
        this.m_TimedSpawnCoroutine = Tools.MyWaitCoroutine(time, null, () => this.Spawn());
        StartCoroutine(this.SpawnRoutine(this.m_TimedSpawnCoroutine));
    }

    /// <summary>
    /// Spawn Routine to wait a courtine for execute 
    /// </summary>
    /// <remarks>You nned to call it with <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/></remarks>
    /// <param name="myWaitCouroutine">The coroutine to wait</param>
    /// <returns>An IEnumerator to execute</returns>
    private IEnumerator SpawnRoutine(IEnumerator myWaitCouroutine)
    {
        yield return myWaitCouroutine;
    }

    /// <summary>
    /// Spawn Each Time in Linear mode and Routine mode 
    /// </summary>
    /// <remarks>You nned to call it with <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/></remarks>
    /// <param name="time">The time</param>
    /// <returns>An IEnumerator to execute</returns>
    private IEnumerator SpawnEachTimeLinearRoutine(float time)
    {
        for (int i = 0; i < this.m_SpawnerTransforms.Length; i++)
        {
            Transform spawnerTransform = this.m_SpawnerTransforms[i];
            float timeMultiplier = i == 0 ? 0f : 1f;

            if (!this.HasReachedSpawnLimit)
            {
                IEnumerator myWaitCouroutine = Tools.MyWaitCoroutine(time * timeMultiplier, null, () => this.Spawn(this.GetRandomObjectToSpawn(), spawnerTransform));
                yield return this.SpawnRoutine(myWaitCouroutine);
            }
        }
    }

    /// <summary>
    /// Spawn each time in the ascending order of the <see cref="m_SpawnerTransforms"/> list
    /// </summary>
    /// <remarks>Execute <see cref="SpawnEachTimeLinearRoutine"/></remarks>
    /// <param name="time">The time between each spawn</param>
    private void SpawnEachTimeLinear(float time)
    {
        this.m_TimedSpawnCoroutine = SpawnEachTimeLinearRoutine(time);
        StartCoroutine(this.m_TimedSpawnCoroutine);
    }
    #endregion

    #region SpawnerManager Utils methods
    /// <summary>
    /// Start the cooldown spawn 
    /// </summary>
    private void StartSpawnCooldown()
    {
        this.m_IsTimedSpawning = true;
    }

    /// <summary>
    /// Stop timed spawn <see cref="SpawnEachTime(float)"/>
    /// </summary>
    private void StopSpawnEachTime()
    {
        if (this.m_TimedSpawnCoroutine != null)
        {
            base.StopCoroutine(this.m_TimedSpawnCoroutine);
            this.m_TimedSpawnCoroutine = null;
        }
    }

    /// <summary>
    /// Stop cooldown spawn
    /// </summary>
    private void StopSpawnCooldown()
    {
        this.m_IsTimedSpawning = false;
    }

    /// <summary>
    /// StopTimedSpawns with <see cref="StopSpawnEachTime"/> and <seealso cref="StopSpawnCooldown"/>
    /// </summary>
    private void StopTimedSpawns()
    {
        this.StopSpawnEachTime();
        this.StopSpawnCooldown();
    }

    /// <summary>
    /// Destroy An Game Object Spawned after it is destroy
    /// </summary>
    /// <param name="gameObjectToDestroy">The GameObject</param>
    private void DestroyAnGameObjectSpawned(GameObject gameObjectToDestroy)
    {
        if (this.m_GameObjectsSpawned.Contains(gameObjectToDestroy))
        {
            this.m_GameObjectsSpawned.Remove(gameObjectToDestroy);
            GameObject.Destroy(gameObjectToDestroy);
        }
    }

    /// <summary>
    /// Destroy all spawned game objects
    /// </summary>
    private void DestroyAllSpawnedGameObjects()
    {
        this.m_GameObjectsSpawned.ForEach((gameObjectSpawned) => GameObject.Destroy(gameObjectSpawned));
        this.m_GameObjectsSpawned.Clear();
    }
    #endregion

    #region SpawnManagers Own Update Methods
    /// <summary>
    /// Update Cooldown Spawn <see cref="StopSpawnCooldown"/> and <seealso cref="SpawnEachNextCooldown"/>
    /// </summary>
    private void UpdateCooldownSpawn()
    {
        if (this.HasReachedSpawnLimit)
        {
            this.StopSpawnCooldown();
        }

        this.SpawnEachNextCooldown();
    }
    #endregion

    #region Events Listeners
    /// <summary>
    /// OnSpawnEachTimeEvent <see cref="SpawnEachTime(float)"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnEachTimeEvent(SpawnEachTimeEvent e)
    {
        this.SpawnEachTime(e.eSpawnTime);
    }

    /// <summary>
    ///  OnSpawnEachTimeLinearEvent <see cref="SpawnEachTimeLinear(float)"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnEachTimeLinearEvent(SpawnEachTimeLinearEvent e)
    {
        this.SpawnEachTimeLinear(e.eSpawnTime);
    }

    /// <summary>
    /// OnSpawnedGameObjectDestroyedEvent <see cref="DestroyAnGameObjectSpawned(GameObject)"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnedGameObjectDestroyedEvent(SpawnedGameObjectToDestroyEvent e)
    {
        this.DestroyAnGameObjectSpawned(e.eGameObjectToDestroy);
    }

    /// <summary>
    /// OnSpawnNbGOEvent <see cref="Spawn(int)"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnNbGOEvent(SpawnNbGOEvent e)
    {
        this.Spawn(e.eNbGOToSpawn);
    }

    /// <summary>
    /// OnSpawnGameObjectEvent <see cref="Spawn(GameObject)"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnGameObjectEvent(SpawnGameObjectEvent e)
    {
        this.Spawn(e.eGameObjectToSpawn);
    }

    /// <summary>
    /// OnSpawnGameObjectsEvent <see cref="Spawn(GameObject)"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnGameObjectsEvent(SpawnGameObjectsEvent e)
    {
        this.Spawn(e.eGameObjectsToSpawn);
    }

    /// <summary>
    /// OnSpawnGameObjectsEvent <see cref="StartSpawnCooldown"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnSpawnGameObjectsEvent(StartCooldownSpawnEvent e)
    {
        this.StartSpawnCooldown();
    }

    /// <summary>
    /// OnStopEachTimeSpawnEvent <see cref="StopSpawnEachTime"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnStopEachTimeSpawnEvent(StopEachTimeSpawnEvent e)
    {
        this.StopSpawnEachTime();
    }

    /// <summary>
    ///  OnStopEachTimeLinearSpawnEvent <see cref="StopSpawnEachTime"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnStopEachTimeLinearSpawnEvent(StopEachTimeLinearSpawnEvent e)
    {
        this.StopSpawnEachTime();
    }

    /// <summary>
    /// OnStopTimedSpawnEvent <see cref="StopTimedSpawns"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnStopTimedSpawnEvent(StopTimedSpawnEvent e)
    {
        this.StopTimedSpawns();
    }
    #endregion

    #region Events Subscription
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<SpawnEachTimeEvent>(OnSpawnEachTimeEvent);
        EventManager.Instance.AddListener<SpawnEachTimeLinearEvent>(OnSpawnEachTimeLinearEvent);
        EventManager.Instance.AddListener<SpawnedGameObjectToDestroyEvent>(OnSpawnedGameObjectDestroyedEvent);
        EventManager.Instance.AddListener<SpawnNbGOEvent>(OnSpawnNbGOEvent);
        EventManager.Instance.AddListener<SpawnGameObjectEvent>(OnSpawnGameObjectEvent);
        EventManager.Instance.AddListener<SpawnGameObjectsEvent>(OnSpawnGameObjectsEvent);
        EventManager.Instance.AddListener<StartCooldownSpawnEvent>(OnSpawnGameObjectsEvent);
        EventManager.Instance.AddListener<StopEachTimeSpawnEvent>(OnStopEachTimeSpawnEvent);
        EventManager.Instance.AddListener<StopEachTimeLinearSpawnEvent>(OnStopEachTimeLinearSpawnEvent);
        EventManager.Instance.AddListener<StopTimedSpawnEvent>(OnStopTimedSpawnEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<SpawnEachTimeEvent>(OnSpawnEachTimeEvent);
        EventManager.Instance.RemoveListener<SpawnEachTimeLinearEvent>(OnSpawnEachTimeLinearEvent);
        EventManager.Instance.RemoveListener<SpawnedGameObjectToDestroyEvent>(OnSpawnedGameObjectDestroyedEvent);
        EventManager.Instance.RemoveListener<SpawnNbGOEvent>(OnSpawnNbGOEvent);
        EventManager.Instance.RemoveListener<SpawnGameObjectEvent>(OnSpawnGameObjectEvent);
        EventManager.Instance.RemoveListener<SpawnGameObjectsEvent>(OnSpawnGameObjectsEvent);
        EventManager.Instance.RemoveListener<StartCooldownSpawnEvent>(OnSpawnGameObjectsEvent);
        EventManager.Instance.RemoveListener<StopEachTimeSpawnEvent>(OnStopEachTimeSpawnEvent);
        EventManager.Instance.RemoveListener<StopEachTimeLinearSpawnEvent>(OnStopEachTimeLinearSpawnEvent);
        EventManager.Instance.RemoveListener<StopTimedSpawnEvent>(OnStopTimedSpawnEvent);
    }
    #endregion

    #region MonoBehaviour Methods
    private void OnEnable()
    {
        this.SubscribeEvents();
    }

    private void OnDisable()
    {
        this.UnsubscribeEvents();
    }

    private void Awake()
    {
        this.m_GameObjectsSpawned = new List<GameObject>();
    }

    private void Start()
    {
        this.m_NextSpawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        this.UpdateCooldownSpawn();
    }

    private void OnDestroy()
    {
        this.DestroyAllSpawnedGameObjects();
    }
    #endregion
}
