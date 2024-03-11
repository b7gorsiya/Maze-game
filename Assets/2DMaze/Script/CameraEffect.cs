using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    Camera cam;
    private void Awake()
    {
        GameController.instanse.ShowLoading();
        cam = Camera.main;
    }

    public void ScaleCamera(float w,float h)
    {
        cam.orthographicSize = w+((w/10)+1f);
        cam.transform.position = new Vector3((w / 2) - 0.5f, (h/2)-0.5f, cam.transform.position.z);
    }
}
