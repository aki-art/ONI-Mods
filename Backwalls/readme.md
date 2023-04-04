Adds a new type of Drywall based on tiles from the game.

# Loading custom textures  
1. Take any image, it should be a tiling square of .png extension. Recommended size is 128x128.
2. Put it in the folder: `\Documents\Klei\OxygenNotIncluded\mods\config\backwalls\custom_walls `

3. (Optional) create a .metadata.json file with the same name as the png, you can configure a border color and a display name here.
For example, for a `cool.png`, there would be a `cool.metadata.json`:
```json
{
    "Name": "My Cool Wallpaper",
    "BorderColorHex": "FF0033"
}
```

Your image should now appear as an option.

![files in the folder](https://i.imgur.com/NOv2W1k.png)  
![the tile loaded in game](https://i.imgur.com/k0rfov2.png)

**I want to change my image, but it's not updating**  
Delete the generated image from `Documents\Klei\OxygenNotIncluded\mods\config\backwalls\generated_textures`