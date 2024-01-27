using System;
using System.Collections.Generic;
using System.Data;
using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

namespace Fizzleon.ECS.Systems;

public class TextureLoaderSystem(ContentManager contentManager) : ContentInitializationSystem(contentManager), ISystem
{
    private readonly List<Entity> menuEntities = [];
    private readonly Dictionary<string, Texture2D> loadedTextures = new();

    public void LoadEntities(IEnumerable<Entity> entities, IEnumerable<string> paths)
    {
        // Iterate over the entities and corresponding paths
        using (var entityEnumerator = entities.GetEnumerator())
        {
            using var pathEnumerator = paths.GetEnumerator();
            while (entityEnumerator.MoveNext() && pathEnumerator.MoveNext())
            {
                ReloadTexture(entityEnumerator.Current, pathEnumerator.Current);
            }
        }
    }

    public List<Rectangle> LoadMenuItems(List<Entity> menuEntities)
    {
        List<Rectangle> buttonsRect = new ();

        foreach (var entity in menuEntities)
        {
            LoadMenuEntity(entity);
            buttonsRect.Add(entity.Get<MenuComponent>().Rectangle);
        }

        return buttonsRect;
    }

    public void ReloadTexture(Entity entity, string path)
    {
        if (entity != null)
        {
            // Check if the entity already has a SpriteComponent
            if (!entity.Has<SpriteComponent>())
            {
                // If not, create and attach a new SpriteComponent
                var spriteComponent = new SpriteComponent(Load<Texture2D>(path));
                entity.Attach(spriteComponent);
            }
            else
            {
                // If it already has a SpriteComponent, retrieve it
                var spriteComponent = entity.Get<SpriteComponent>();

                // Load the texture and set it for the SpriteComponent
                var loadedTexture = Load<Texture2D>(path);
                loadedTextures[path] = loadedTexture;

                // Make sure the SpriteComponent's Texture property is updated
                spriteComponent.Texture = loadedTexture;
            }
        }
        else
        {
            // Handle the case where entity is null
            throw new InvalidOperationException("Entity is null.");
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
                menuEntity.Attach(spriteComponent); // Attach SpriteComponent to the entity
            }
            else
            {
                // If it already has a SpriteComponent, retrieve it
                spriteComponent = menuEntity.Get<SpriteComponent>();
            }

            // Load the texture and set it for the SpriteComponent
            var loadedTexture = Load<Texture2D>(menuComponent.Texture.Name);
            loadedTextures[menuComponent.Texture.Name] = loadedTexture;

            // Make sure the SpriteComponent's Texture property is updated
            spriteComponent.Texture = loadedTexture;
        }
    }

    public void LoadButton(Entity entity, Texture2D texture)
    {
        if (entity != null && texture != null)
        {
            var spriteComponent = entity.Get<SpriteComponent>();
            if (spriteComponent != null)
            {
                spriteComponent.Texture = texture; // Set the loaded texture
            }
            else
            {
                // Handle the case where SpriteComponent is not attached
                throw new InvalidOperationException("SpriteComponent is not attached to the entity.");
            }
        }
        else
        {
            // Handle the case where entity or its texture is null
            throw new InvalidOperationException("Entity or its texture is null.");
        }
    }
    public AnimatedSprite LoadAnimation(string pathToXnb, string defaultAnimation = null)
    {
        SpriteSheet spriteSheet = Load<SpriteSheet>(pathToXnb, new JsonContentLoader());

        AnimatedSprite animatedSprite = new AnimatedSprite(spriteSheet, defaultAnimation);

        return animatedSprite;
    }


    public Texture2D LoadTransitionTexture(string path)
    {
        Load<Texture2D>(path);

        if (!loadedTextures.ContainsKey(path))
        {
            var loadedTexture = Load<Texture2D>(path);
            loadedTextures[path] = loadedTexture;
        }

        return loadedTextures[path];
    }

    public new void Initialize(World world)
    {

    }
    public new void Dispose()
    {
        foreach (var loadedTexture in loadedTextures.Values)
        {
            loadedTexture.Dispose();
        }

        loadedTextures.Clear();
        Content.Unload();
    }
}