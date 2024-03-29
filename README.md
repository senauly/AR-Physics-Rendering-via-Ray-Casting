# AR: Physics Rendering via Ray Casting
I developed a  simple  renderer  using  ray  casting following this rules:
  1. Build  a  3D  world  in  Unity with  at  least 4 separate  objects with  adjustable  pose.
  2. Add  at  least  three  different light sources
  3. The camera is a pinhole camera with adjustable FoV, center and viewing directions.
  
 ## Simple Renderer Algorithm

The script generates a texture image by casting rays from each pixel in the image towards the game world and calculating the color of each pixel based on the Phong lighting model.

The Phong lighting model takes into account the material's diffuse color (color of the material under diffuse), the specular color (color of the highlights on the material), and the ambient light in the scene (diffuse light that is present everywhere in the scene).

Create shadow function is called for each pixel that is hit by a ray. This function sends a ray from the point of the hit towards each light in the scene. If the ray does not hit the light, it means that the pixel is in shadow and it calculates a shadow color based on the intensity of the light and then sets the pixel color to the shadow color.

After adding the shadows and applying the changes to the RawImage, it saves an image of the result.

## Results

### The Scene
<img width="793" alt="image" src="https://user-images.githubusercontent.com/69864434/232149206-53d672b5-04e5-45d1-a701-7f4cab6fcd6b.png">

### The Result Image
<img width="793" alt="image" src="https://user-images.githubusercontent.com/69864434/232149559-f4f03c91-622a-4260-854e-62af1a301340.png">
