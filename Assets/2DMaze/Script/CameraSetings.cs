using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetings : MonoBehaviour
{
    private float DesignOrthographicSize=6;
    private float DesignAspect;
    private float DesignWidth;

    public float DesignAspectHeight;
    public float DesignAspectWidth;
    Camera cam;

    public void Awake()
    {
        cam = Camera.main;
        this.DesignOrthographicSize = cam.orthographicSize;
        this.DesignAspect = this.DesignAspectHeight / this.DesignAspectWidth;
        this.DesignWidth = this.DesignOrthographicSize * this.DesignAspect;

        this.Resize();
    }

    public void Resize()
    {
        float wantedSize = this.DesignWidth / cam.aspect;
        cam.orthographicSize = Mathf.Max(wantedSize,
            this.DesignOrthographicSize);
    }
   
}
