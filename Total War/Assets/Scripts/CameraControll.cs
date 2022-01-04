using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public float panspeed = 20f;
    public float panboardthick = 30f;
    public Vector2 panlimit;
    public float zoomspeed = 2f;
    public float smoothspeed = 2f;
    public float minZoom = 1f;
    public float maxZoom = 20f;
    private float targetCam;
    void Update()
    {
        Vector3 pos = transform.position;
        
        if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panboardthick)
        {
            pos.y += panspeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <=  panboardthick)
        {
            pos.y -= panspeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panboardthick)
        {
            pos.x += panspeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <=  panboardthick)
        {
            pos.x -= panspeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, -panlimit.x, panlimit.x);
        pos.y = Mathf.Clamp(pos.y, -panlimit.y, panlimit.y);

        transform.position = pos;

        targetCam = Camera.main.orthographicSize;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if ( scroll != 0f)
        {
            targetCam -= scroll * zoomspeed;
            targetCam = Mathf.Clamp(targetCam, minZoom, maxZoom);
        }
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetCam, smoothspeed * Time.deltaTime);
    }

    
    }

