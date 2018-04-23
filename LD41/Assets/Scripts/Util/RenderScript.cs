using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class RenderScript : MonoBehaviour {

    public GameObject obj;
    public void Start()
    {
        Texture2D tex = AssetPreview.GetMiniThumbnail(obj);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "DylanScreenShot1.png", bytes);
    }

    //public Camera cam;

    //void Update()
    //{
    //    if (Input.GetKeyDown("e"))
    //    {
    //        Debug.Log("Saving Screen Shot");
    //        StartCoroutine(CreateLayerThumbnail());
    //    }

    //}

    //IEnumerator CreateLayerThumbnail()
    //{
    //    yield return new WaitForEndOfFrame();

    //    // create a texture to pass to encoding
    //    Texture2D texture = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);

    //    // Initialize and render
    //    cam.Render();
    //    RenderTexture.active = cam.targetTexture;

    //    // put buffer into texture
    //    texture.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);

    //    yield return 0;

    //    texture.Apply();

    //    yield return 0;

    //    byte[] bytes = texture.EncodeToPNG();

    //    // save the image
    //    string imagePath = "amazingPath.png";
    //    File.WriteAllBytes(imagePath, bytes);

    //    //Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
    //    DestroyObject(texture);
    //}
}
