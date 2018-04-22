using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    public float maxZoom = 100f;
    public float minZoom = 30f;

    public float zoomSpeed = 1f;
    public float scrollSpeed = 1f;

    public float edgeDistanceForMouseScroll = 5f;

    public Camera mainCamera;

	// Update is called once per frame
	void Update () {

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (mainCamera.fieldOfView > minZoom)
                mainCamera.fieldOfView -= zoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (mainCamera.fieldOfView < maxZoom)
                mainCamera.fieldOfView += zoomSpeed;
        }

        //Debug.Log(Input.mousePosition);
        //Debug.Log(Screen.width + " " + Screen.height);

        if (Input.mousePosition.x < edgeDistanceForMouseScroll || Input.GetKey(KeyCode.A))
            mainCamera.transform.Translate(-scrollSpeed*Time.deltaTime, 0, 0, Space.World);

        if (Input.mousePosition.y < edgeDistanceForMouseScroll || Input.GetKey(KeyCode.S))
            mainCamera.transform.Translate(0, 0, -scrollSpeed*Time.deltaTime, Space.World);

        if (Input.mousePosition.x > Screen.width - edgeDistanceForMouseScroll || Input.GetKey(KeyCode.D))
            mainCamera.transform.Translate(scrollSpeed*Time.deltaTime, 0, 0, Space.World);

        if (Input.mousePosition.y > Screen.height - edgeDistanceForMouseScroll || Input.GetKey(KeyCode.W))
            mainCamera.transform.Translate(0, 0, scrollSpeed*Time.deltaTime, Space.World);
	}
}
