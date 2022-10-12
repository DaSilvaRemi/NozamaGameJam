// scrolls a quad object
using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour
{
    public float scrollSpeed;
    public GameObject prefab;

    private float length;
    private Vector3 startPos;
    private Quaternion startRot;
    private GameObject clone;

    void Awake()
    {
        length = GetComponent<BoxCollider2D>().bounds.size.x;
        startPos = transform.position;
        startRot = transform.rotation;
        clone = Instantiate(prefab, new Vector3(startPos.x + length, startPos.y, startPos.z), startRot);
        clone.transform.localScale = transform.localScale;
    }

    void Update()
    {
        prefab.transform.position += Vector3.left * Time.deltaTime * scrollSpeed;
        clone.transform.position += Vector3.left * Time.deltaTime * scrollSpeed;
        if (clone.transform.position.x <= startPos.x)
        {
            prefab.transform.position = startPos;
            clone.transform.position = new Vector3(startPos.x + length, startPos.y, startPos.z);
        }
    }
}