using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float moveSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MoveIfKey(KeyCode.W, new Vector3(0, moveSpeed, 0));
        MoveIfKey(KeyCode.S, new Vector3(0, -moveSpeed, 0));
        MoveIfKey(KeyCode.D, new Vector3(moveSpeed, 0));
        MoveIfKey(KeyCode.A, new Vector3(-moveSpeed, 0));

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(TileManager.Instance.worldToCoords(transform.position));
        }
    }

    void MoveIfKey(KeyCode key, Vector3 delta)
    {
        if (Input.GetKey(key))
            this.transform.position = this.transform.position + delta;
    }
}
