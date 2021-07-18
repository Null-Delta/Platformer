using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedObject : StaticMapObject
{
    

    public void setupStyle(int x, int y) {
        int sum = 0;
        
    

        if (map.getMapObjects<StaticMapObject>(x - 1, y + 1, x => x.objectName == objectName) != null) sum = sum | 0b00000010;
        if (map.getMapObjects<StaticMapObject>(x, y + 1, x => x.objectName == objectName) != null)     sum = sum | 0b00000100;
        if (map.getMapObjects<StaticMapObject>(x + 1, y + 1, x => x.objectName == objectName) != null) sum = sum | 0b00001000;
        if (map.getMapObjects<StaticMapObject>(x + 1, y, x => x.objectName == objectName) != null)     sum = sum | 0b00010000;
        if (map.getMapObjects<StaticMapObject>(x + 1, y - 1, x => x.objectName == objectName) != null) sum = sum | 0b00100000;
        if (map.getMapObjects<StaticMapObject>(x, y - 1, x => x.objectName == objectName) != null)     sum = sum | 0b01000000;
        if (map.getMapObjects<StaticMapObject>(x - 1, y - 1, x => x.objectName == objectName) != null) sum = sum | 0b10000000;
        if (map.getMapObjects<StaticMapObject>(x - 1, y, x => x.objectName == objectName) != null)     sum = sum | 0b00000001;
        generateTexture(sum);
    }

    void generateTexture(int code) {

        List<Texture2D> textures = new List<Texture2D>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/"+ objectName + "s");
        for (int i = 0; i < 20; i++)
        {
            var croppedTexture = new Texture2D( (int) sprites[i].rect.width, (int) sprites[i].rect.height );
            var pixels = sprites[i].texture.GetPixels(  (int) sprites[i].textureRect.x, 
                                                    (int) sprites[i].textureRect.y, 
                                                    (int) sprites[i].textureRect.width, 
                                                    (int) sprites[i].textureRect.height );
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();
            textures.Add(croppedTexture);
        }
        int widht = (int)(gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 17);
        int height = (int)(gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 17);
        



        Texture2D texture = new Texture2D(widht,height);

        for(int x = 0; x < widht; x++) {
            for(int y = 0; y < height; y++) {
                texture.SetPixel(x,y,new Color(0,0,0,0));
            }
        }
        texture.filterMode = FilterMode.Point;
        


        



        
        switch ((code & 0b00000111)) {
            case 0b00000000:
            case 0b00000010:
            drawOn(ref texture, textures[0], widht, height);
            break;
            case 0b00000001:
            case 0b00000011:
            drawOn(ref texture, textures[1], widht, height);
            break;
            case 0b00000100:
            case 0b00000110:
            drawOn(ref texture, textures[2], widht, height);
            break;
            case 0b00000101:
            drawOn(ref texture, textures[3], widht, height);
            break;
            case 0b00000111:
            drawOn(ref texture, textures[4], widht, height);
            break;
        }

        switch ((code & 0b00011100)) {
            case 0b00001000:
            case 0b00000000:
            drawOn(ref texture, textures[5], widht, height);
            break;
            case 0b00000100:
            case 0b00001100:
            drawOn(ref texture, textures[7], widht, height);
            break;
            case 0b00010000:
            case 0b00011000:
            drawOn(ref texture, textures[6], widht, height);
            break;
            case 0b00010100:
            drawOn(ref texture, textures[8], widht, height);
            break;
            case 0b00011100:
            drawOn(ref texture, textures[9], widht, height);
            break;
        }

        switch ((code & 0b01110000)) {
            case 0b00100000:
            case 0b00000000:
            drawOn(ref texture, textures[10], widht, height);
            break;
            case 0b00010000:
            case 0b00110000:
            drawOn(ref texture, textures[11], widht, height);
            break;
            case 0b01000000:
            case 0b01100000:
            drawOn(ref texture, textures[12], widht, height);
            break;
            case 0b01010000:
            drawOn(ref texture, textures[13], widht, height);
            break;
            case 0b01110000:
            drawOn(ref texture, textures[14], widht, height);
            break;
        }

        switch ((code & 0b11000001)) {
            case 0b10000000:
            case 0b00000000:
            drawOn(ref texture, textures[15], widht, height);
            break;
            case 0b01000000:
            case 0b11000000:
            drawOn(ref texture, textures[17], widht, height);
            break;
            case 0b00000001:
            case 0b10000001:
            drawOn(ref texture, textures[16], widht, height);
            break;
            case 0b01000001:
            drawOn(ref texture, textures[18], widht, height);
            break;
            case 0b11000001:
            drawOn(ref texture, textures[19], widht, height);
            break;
        }

        Sprite s = Sprite.Create(texture, new Rect(0,0, widht, height), new Vector2(1f,0.5f), 17);
        
        gameObject.GetComponent<SpriteRenderer>().sprite = s;
    }
    void drawOn(ref Texture2D main, Texture2D sorse, int widht, int height) {
        for(int x = 0; x < widht; x++) {
            for(int y = 0; y < height; y++) {
                if(main.GetPixel(x,y).a == 0)
                    main.SetPixel(x,y,sorse.GetPixel(x,y));
            }
        }
        main.Apply();
    }
}
