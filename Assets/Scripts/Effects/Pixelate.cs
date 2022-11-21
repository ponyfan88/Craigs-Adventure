/* 
 * Programmers: Jack Kennedy
 * Purpose: generate a new rendertexture based on our resolution
 * Inputs: a camera and a rawimage
 * Outputs: a new rendertexture
 */

using UnityEngine;
using UnityEngine.UI;

public class Pixelate : MonoBehaviour
{
    #region Variables

    // resolution instantialization
    private Vector2 resolution;
    // aspect ratio instantialization
    private float aspect;

    // import the camera
    public new Camera camera; // i have no idea why we use new here. its not like this is a new camera...
    // import the rawimage
    public RawImage rawImage;
    // let us set the resolution of the rendertexture in the inspector
    public int res;

    #endregion

    #region Default Methods

    // on start
    private void Start()
    {
        // get our resolution on awake
        resolution = new Vector2(Screen.width, Screen.height);

        // set our NEW resolution
        resolution.x = Screen.width;
        resolution.y = Screen.height;

        // calculate aspect ratio
        aspect = resolution.x / resolution.y;


        // our final resolution
        int finalRes = res;

        // if we are on the final floor
        if (FloorManager.floor >= 4)
        {
            // multiply our resolution by 2
            finalRes *= 2;
        }

        // create a new render texture based on our aspect ratio
        var renderTexture = new RenderTexture((int)(finalRes * aspect), finalRes, 24);

        // use pixelate filter
        renderTexture.filterMode = FilterMode.Point;

        // use our new renderTexture
        camera.targetTexture = renderTexture;
        rawImage.texture = renderTexture;
    }

    // on update
    private void Update()
    {
        // if we change our resolution
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            // set our NEW resolution
            resolution.x = Screen.width;
            resolution.y = Screen.height;

            // calculate aspect ratio
            aspect = resolution.x / resolution.y;

            // our final resolution
            int finalRes = res;

            // if we are on the final floor
            if (FloorManager.floor >= 4)
            {
                // multiply our resolution by 2
                finalRes *= 2;
            }

            // create a new render texture based on our aspect ratio
            var renderTexture = new RenderTexture((int)(finalRes * aspect), finalRes, 24);

            // use pixelate filter
            renderTexture.filterMode = FilterMode.Point;

            // use our new renderTexture
            camera.targetTexture = renderTexture;
            rawImage.texture = renderTexture;
        }
    }

    #endregion
}
