
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using System.Collections.Generic;

namespace Fizzleon.ECS.Systems;
public class ContentInitializationSystem : ISystem
{
    private readonly ContentManager contentManager;
    private readonly Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();


    private ContentInitializationSystem(ContentManager contentManager)
    {
        this.contentManager = contentManager;
    }

    public static ContentInitializationSystem Create(ContentManager contentManager) => new ContentInitializationSystem(contentManager);




    public void LoadTextures(IEnumerable<string> texturePaths)
    {
        foreach (var path in texturePaths)
        {
            if (!loadedTextures.ContainsKey(path))
            {
                var loadedTexture = contentManager.Load<Texture2D>(path);
                loadedTextures[path] = loadedTexture;
            }
        }
    }
    public T Load<T>(string assetName)
    {
        return contentManager.Load<T>(assetName);
    }
    public SpriteSheet LoadSpriteSheet(string assetName, IContentLoader contentLoader)
    {
        return contentManager.Load<SpriteSheet>(assetName, contentLoader);
    }
    public void Dispose()
    {
        foreach (var loadedTexture in loadedTextures.Values)
        {
            loadedTexture.Dispose();
        }


        loadedTextures.Clear();
        contentManager.Unload();
    }

    public void Initialize(World world)
    {
        // You can perform additional initialization here if needed
    }
}
