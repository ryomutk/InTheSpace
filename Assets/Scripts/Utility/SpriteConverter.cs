using UnityEngine;
using System.Collections.Generic;

public static class SpriteConverter
{
    static Dictionary<TextureData, Texture2D> createdTexture = new Dictionary<TextureData, Texture2D>();
    public static TextureData SpriteToData(Sprite sprite)
    {
        var data = new TextureData();
        data.data = sprite.texture.GetRawTextureData();
        data.format = sprite.texture.format;

        var rawPivot = sprite.pivot;
        data.size = sprite.textureRect.size;
        rawPivot.x /= sprite.texture.width;
        rawPivot.y /= sprite.texture.height;
        data.pivot = rawPivot;
        data.pixelsPerUnit = sprite.pixelsPerUnit;
        return data;
    }


    public static Sprite DataToSprite(TextureData data)
    {
        Texture2D tex;
        if (createdTexture.ContainsKey(data))
        {
            tex = createdTexture[data];

        }
        else
        {
            //多分このあとLoadするので初期化値は関係ない？
            tex = new Texture2D((int)data.size.x, (int)data.size.y, data.format, false);

            tex.LoadRawTextureData(data.data);
            tex.Apply();
        }

        Rect texRect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, texRect, data.pivot, data.pixelsPerUnit);



        return sprite;
    }
}

