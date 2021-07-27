using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedObject : MapObject
{
    const int width = 25, height = 59;
    static List<string> already_generated_names = new List<string>();
    static List<Sprite[]> ready_sprites = new List<Sprite[]>();

    public void setupStyle(int x, int y) {
        int sum = 0;
        bool is_generated = false;
        int ind;
        
        for (ind = 0; ind != already_generated_names.Count; ind++)
            if (already_generated_names[ind] == objectName)
            {
                is_generated = true;
                break;
            }
        
        if (!is_generated)
        {
            already_generated_names.Add(objectName);
            ready_sprites.Add(new Sprite[256]);
            is_generated = true;
        }

        if (is_generated)
        {
            if (map.getMapObjects<MapObject>(x - 1, y + 1, x => x.objectName == objectName) != null) sum = sum | 0b00000010;
            if (map.getMapObjects<MapObject>(x, y + 1, x => x.objectName == objectName) != null)     sum = sum | 0b00000100;
            if (map.getMapObjects<MapObject>(x + 1, y + 1, x => x.objectName == objectName) != null) sum = sum | 0b00001000;
            if (map.getMapObjects<MapObject>(x + 1, y, x => x.objectName == objectName) != null)     sum = sum | 0b00010000;
            if (map.getMapObjects<MapObject>(x + 1, y - 1, x => x.objectName == objectName) != null) sum = sum | 0b00100000;
            if (map.getMapObjects<MapObject>(x, y - 1, x => x.objectName == objectName) != null)     sum = sum | 0b01000000;
            if (map.getMapObjects<MapObject>(x - 1, y - 1, x => x.objectName == objectName) != null) sum = sum | 0b10000000;
            if (map.getMapObjects<MapObject>(x - 1, y, x => x.objectName == objectName) != null)     sum = sum | 0b00000001;
            
            if (ready_sprites[ind][sum] != null)
                gameObject.GetComponent<SpriteRenderer>().sprite = ready_sprites[ind][sum];
            else
            {
                ready_sprites[ind][sum] = generateTexture(sum);
                gameObject.GetComponent<SpriteRenderer>().sprite = ready_sprites[ind][sum];
            }
            //generateTexture(sum);
        }
    }

    Sprite generateTexture(int code) {
        List<Texture2D> textures = new List<Texture2D>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/"+ objectName);
        for (int i = 0; i < 20; i++)
        {
            var croppedTexture = new Texture2D(width, height);

            var pixels = sprites[i].texture.GetPixels(  (int) sprites[i].textureRect.x, 
                                                    (int) sprites[i].textureRect.y, 
                                                    width, 
                                                    height);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();
            textures.Add(croppedTexture);
        }
        Texture2D texture = new Texture2D(width,height);

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                texture.SetPixel(x,y,new Color(0,0,0,0));
            }
        }

        texture.filterMode = FilterMode.Point;
        
        switch ((code & 0b00000111)) {
            case 0b00000000:
            case 0b00000010:
            drawOn(ref texture, textures[0]);
            break;
            case 0b00000001:
            case 0b00000011:
            drawOn(ref texture, textures[1]);
            break;
            case 0b00000100:
            case 0b00000110:
            drawOn(ref texture, textures[2]);
            break;
            case 0b00000101:
            drawOn(ref texture, textures[3]);
            break;
            case 0b00000111:
            drawOn(ref texture, textures[4]);
            break;
        }

        switch ((code & 0b00011100)) {
            case 0b00001000:
            case 0b00000000:
            drawOn(ref texture, textures[5]);
            break;
            case 0b00000100:
            case 0b00001100:
            drawOn(ref texture, textures[7]);
            break;
            case 0b00010000:
            case 0b00011000:
            drawOn(ref texture, textures[6]);
            break;
            case 0b00010100:
            drawOn(ref texture, textures[8]);
            break;
            case 0b00011100:
            drawOn(ref texture, textures[9]);
            break;
        }

        switch ((code & 0b01110000)) {
            case 0b00100000:
            case 0b00000000:
            drawOn(ref texture, textures[10]);
            break;
            case 0b00010000:
            case 0b00110000:
            drawOn(ref texture, textures[11]);
            break;
            case 0b01000000:
            case 0b01100000:
            drawOn(ref texture, textures[12]);
            break;
            case 0b01010000:
            drawOn(ref texture, textures[13]);
            break;
            case 0b01110000:
            drawOn(ref texture, textures[14]);
            break;
        }

        switch ((code & 0b11000001)) {
            case 0b10000000:
            case 0b00000000:
            drawOn(ref texture, textures[15]);
            break;
            case 0b01000000:
            case 0b11000000:
            drawOn(ref texture, textures[17]);
            break;
            case 0b00000001:
            case 0b10000001:
            drawOn(ref texture, textures[16]);
            break;
            case 0b01000001:
            drawOn(ref texture, textures[18]);
            break;
            case 0b11000001:
            drawOn(ref texture, textures[19]);
            break;
        }
        
        Sprite s = Sprite.Create(texture, new Rect(0,0, width, height), new Vector2(0.5f,0.5f), 17);
        return s;
    }

    void drawOn(ref Texture2D main, Texture2D sorse) {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                if(sorse.GetPixel(x,y).a != 0 && main.GetPixel(x,y).a == 0)
                    main.SetPixel(x,y,sorse.GetPixel(x,y));
            }
        }
        main.Apply();
    }
}
