using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    Transform player;

    private void Update()
    {
        Vector3 pos = new Vector3(player.position.x, player.position.y, -1);
        cam.transform.position = pos;
    }
}
