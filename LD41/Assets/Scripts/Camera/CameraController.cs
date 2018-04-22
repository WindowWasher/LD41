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

    private Camera mainCamera;
    private float originalCameraHeight;

    private void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        originalCameraHeight = mainCamera.transform.position.y;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (mainCamera.transform.position.y > originalCameraHeight - minZoom)
                mainCamera.transform.Translate(0, 0, zoomSpeed*Time.deltaTime);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (mainCamera.transform.position.y < originalCameraHeight + maxZoom)
                mainCamera.transform.Translate(0, 0, -zoomSpeed*Time.deltaTime);
        }

        Vector3 localForward = mainCamera.transform.forward;
        localForward.y = 0;
        localForward.Normalize();

        if (Input.mousePosition.x < edgeDistanceForMouseScroll || Input.GetKey(KeyCode.A))
            mainCamera.transform.position += mainCamera.transform.right * -scrollSpeed * Time.deltaTime;

        if (Input.mousePosition.y < edgeDistanceForMouseScroll || Input.GetKey(KeyCode.S))
            mainCamera.transform.position += localForward * -scrollSpeed * Time.deltaTime;

        if (Input.mousePosition.x > Screen.width - edgeDistanceForMouseScroll || Input.GetKey(KeyCode.D))
            mainCamera.transform.position += mainCamera.transform.right * scrollSpeed * Time.deltaTime;

        if (Input.mousePosition.y > Screen.height - edgeDistanceForMouseScroll || Input.GetKey(KeyCode.W))
            mainCamera.transform.position += localForward * scrollSpeed * Time.deltaTime;

        
	}
}
