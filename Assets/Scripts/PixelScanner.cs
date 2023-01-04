using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PixelScanner : MonoBehaviour
{
    private Texture2D texture;
    public RawImage rawImage;
    public Light[] lights;

    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(Camera.main.pixelWidth, Camera.main.pixelHeight);
        CreateTexture();
        
        // apply the changes
        texture.Apply();
        rawImage.texture = texture;
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes("./image.png", bytes);
    }

    void CreateTexture()
    {
        // Loop through all of the pixels in the texture
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                // get the raycast from the pixel
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(i, j, 0));

                // if the raycast hits something
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Get the color of the material
                    Color materialColor = hit.collider.GetComponent<Renderer>().material.GetColor("_Color");

                    // Calculate the final color of the pixel using the Phong lighting model
                    Color finalColor = materialColor * RenderSettings.ambientLight;

                    // Loop through all of the light sources in the scene
                    foreach (Light light in lights)
                    {
                        // Calculate the direction of the light source
                        Vector3 lightDirection = light.transform.position - hit.point;
                        lightDirection.Normalize();

                        // Calculate the diffuse reflection
                        float diffuseFactor = Mathf.Max(0, Vector3.Dot(lightDirection, hit.normal));
                        Color diffuseReflection = diffuseFactor * materialColor * light.color;

                        // Add the diffuse reflection to the final color
                        finalColor += diffuseReflection;

                    }

                    // Set the pixel color to the final color
                    texture.SetPixel(i, j, finalColor);
                    CreateShadow(i, j, hit);

                }
            }

        }
    }

    void CreateShadow(int i, int j, RaycastHit hit)
    {
        // Send a raycast from the point of hit to the light
        foreach (Light light in lights)
        {
            if (Physics.Raycast(hit.point, light.transform.position - hit.point, out RaycastHit lightHit))
            {
                if (lightHit.collider.gameObject != light.transform.gameObject && texture.GetPixel(i, j) == hit.collider.GetComponent<Renderer>().material.color)
                {
                    // The raycast does not hit the light, so the pixel is in shadow
                    // Calculate the shadow color based on the intensity of the light
                    Color shadowColor = Color.grey*light.intensity;
                    // Set the pixel color to the shadow color
                    texture.SetPixel(i, j, shadowColor);
                }
            }
        }
    }

}