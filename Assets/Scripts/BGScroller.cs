// scrolls a quad object
using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour
{
    public float scrollSpeed;
    public GameObject prefab;
    public GameObject player;

    private float length;
    private Vector3 startPos;
    private Vector3 playerPos;
    private Quaternion startRot;
    private GameObject clone;

    void Awake()
    {
        length = GetComponent<Collider>().bounds.size.z;
        startPos = transform.position;
        startRot = transform.rotation;
        playerPos = player.transform.position;
        clone = Instantiate(prefab, new Vector3(startPos.x, startPos.y, startPos.z + length), startRot);
        clone.transform.localScale = transform.localScale;
    }

    void Update()
    {
        if (playerPos != player.transform.position)
        {
            prefab.transform.position += Vector3.back * Time.deltaTime * scrollSpeed;
            clone.transform.position += Vector3.back * Time.deltaTime * scrollSpeed;
        }
        if (player.transform.position.z > clone.transform.position.z)
        {
            prefab.transform.position = new Vector3(startPos.x, startPos.y, startPos.z + +player.transform.position.z);
            clone.transform.position = new Vector3(startPos.x, startPos.y, startPos.z + length + player.transform.position.z);
        }
        playerPos = player.transform.position;
    }
}