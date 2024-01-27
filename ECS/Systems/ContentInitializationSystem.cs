
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using System.Collections.Generic;
using System.Net.Http.Json;
using MonoGame.Extended.Serialization;
using System;

namespace Fizzleon.ECS.Systems;
public class ContentInitializationSystem : IDisposable
{
    private readonly Dictionary<string, Texture2D> loadedTextures = new();
    private protected ContentManager Content;

    public ContentInitializationSystem(ContentManager content) => Content = content;

    public static ContentInitializationSystem Create(ContentManager content)
    {
        return new ContentInitializationSystem(content);
    }

    public T Load<T>(string path, IContentLoader contentLoader)
    {

        return contentLoader.Load<T>(Content, path);
    }
    public T Load<T>(string assetName)
    {
        return Content.Load<T>(assetName);
    }

    public void Dispose()
    {
        foreach (var loadedTexture in loadedTextures.Values)
        {
            loadedTexture.Dispose();
        }

        loadedTextures.Clear();
        Content.Unload();
    }
    public void Initialize(World world)
    {
    }
}
