shader_type canvas_item;
// Brightness (less than 1 is darker, above 1 is brighter)
uniform float brightness = 1.13;
void fragment()
{
    // Get pixel color from screen
    vec4 pixel_color = vec4(texture(SCREEN_TEXTURE, SCREEN_UV));
    //vec3 pixel_color = texscreen(SCREEN_UV * TEXTURE_PIXEL_SIZE);

    // Get pixel color from texture (set on the TextureFrame)
    vec4 grid_pixel_color = texture(TEXTURE, UV);

    // If the pixel color from our grid image is dark, set it to be fully transparent
    if(grid_pixel_color.r < 0.5) {
   	COLOR = vec4(1,1,1,0); // (red, green, blue, alpha-transparency)
    } else { // Since it is NOT dark, we adjust the pixel color brightness
   	COLOR = pixel_color * brightness;
    }
}