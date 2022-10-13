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
    private Quaternion startRot;
    private GameObject clone;

    void Awake()
    {
        length = GetComponent<Collider>().bounds.size.z;
        startPos = transform.position;
        startRot = transform.rotation;
        clone = Instantiate(prefab, new Vector3(startPos.x, startPos.y, startPos.z + length), startRot);
        clone.transform.localScale = transform.localScale;
    }

    void Update()
    {
        prefab.transform.position += Vector3.back * Time.deltaTime * scrollSpeed;
        clone.transform.position += Vector3.back * Time.deltaTime * scrollSpeed;
        if (clone.transform.position.z <= startPos.z)
        {
            prefab.transform.position = startPos;
            clone.transform.position = new Vector3(startPos.x, startPos.y, startPos.z + length);
        }
    }
}