using Fizzleon.ECS.Components;
using Fizzleon.ECS.Systems;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;

public class TextureLoaderSystem : ISystem
{
    private readonly ContentInitializationSystem contentInitializationSystem;

    private readonly List<Entity> menuEntities = new List<Entity>();

    private readonly Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();

    public static TextureLoaderSystem Create(ContentInitializationSystem contentInitializationSystem) => new TextureLoaderSystem(contentInitializationSystem);

    private TextureLoaderSystem(ContentInitializationSystem contentInitializationSystem)
    {
        this.contentInitializationSystem = contentInitializationSystem;
    }


    public void LoadEntities(IEnumerable<Entity> entities, IEnumerable<string> paths)
    {
        // Iterate over the entities and corresponding paths
        using (var entityEnumerator = entities.GetEnumerator())
        using (var pathEnumerator = paths.GetEnumerator())
        {
            while (entityEnumerator.MoveNext() && pathEnumerator.MoveNext())
            {
                LoadTexture(entityEnumerator.Current, pathEnumerator.Current);
            }
        }
    }

    public List<Rectangle> LoadMenuItems(List<Entity> menuEntities)
    {
        List<Rectangle> buttonsRect = new List<Rectangle>();

        foreach (var entity in menuEntities)
        {
            LoadMenuEntity(entity);
            buttonsRect.Add(entity.Get<MenuComponent>().Rectangle);
        }

        return buttonsRect;
    }

    // Additional method to add entities to the menuEntities list
    public void AddMenuEntity(Entity entity)
    {
        menuEntities.Add(entity);
    }
    public Texture2D LoadTexture(Entity entity, string path)
    {
        if (entity != null)
        {
            var spriteComponent = entity.Get<SpriteComponent>();
            if (spriteComponent != null)
            {
                return spriteComponent.Texture; // Return the loaded texture
            }
            else
            {
                // Handle the case where SpriteComponent is not attached
                return null; // or return a default texture, or throw an exception, depending on your design
            }
        }
        else
        {
            // Handle the case where entity is null
            return null; // or return a default texture, or throw an exception, depending on your design
        }
    }
    private void LoadMenuEntity(Entity menuEntity)
    {
        var menuComponent = menuEntity.Get<MenuComponent>();

        if (!loadedTextures.ContainsKey(menuComponent.Texture.Name))
        {
            SpriteComponent spriteComponent;

            // Check if the entity already has a SpriteComponent
            if (!menuEntity.Has<SpriteComponent>())
            {
                // If not, create and attach a new SpriteComponent
                spriteComponent = new SpriteComponent(null);
                menuEntity.Attach(spriteComponent);
            }
            else
            {
                // If it already has a SpriteComponent, retrieve it
                spriteComponent = menuEntity.Get<SpriteComponent>();
            }

            // Load the texture and set it for the SpriteComponent
            var loadedTexture = contentInitializationSystem.Load<Texture2D>(menuComponent.Texture.Name);
            loadedTextures[menuComponent.Texture.Name] = loadedTexture;

            // Make sure the SpriteComponent's Texture property is updated
            spriteComponent.Texture = loadedTexture;
        }
    }

    private void LoadAnimation(AnimationComponent animationComponent)
    {
        animationComponent.SpriteSheet = contentInitializationSystem.LoadSpriteSheet(animationComponent.PathToSF, new JsonContentLoader());
        animationComponent.AnimatedSprite = new AnimatedSprite(animationComponent.SpriteSheet);
    }

    public void Dispose()
    {
        foreach (var loadedTexture in loadedTextures.Values)
        {
            loadedTexture.Dispose();
        }

        loadedTextures.Clear();
    }
    public Texture2D LoadTransitionTexture(string path)
    {
        contentInitializationSystem.Load<Texture2D>(path);

        if (!loadedTextures.ContainsKey(path))
        {
            var loadedTexture = contentInitializationSystem.Load<Texture2D>(path);
            loadedTextures[path] = loadedTexture;
        }

        return loadedTextures[path];
    }

    public void Initialize(World world)
    {
    }
}
