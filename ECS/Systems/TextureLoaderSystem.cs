using Fizzleon.ECS.Components;
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;

public class TextureLoaderSystem : ISystem
{
    private readonly ContentManager contentManager;
    private readonly Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();

    // Declare the menuEntities list as a private field
    private readonly List<Entity> menuEntities = new List<Entity>();

    public static TextureLoaderSystem Create(ContentManager contentManager)
    {
        return new TextureLoaderSystem(contentManager);
    }

    private TextureLoaderSystem(ContentManager contentManager)
    {
        this.contentManager = contentManager;
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

    public Texture2D LoadTexture(Entity entity, string path = null)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null when loading texture.");
        }

        // Check if the entity has a SpriteComponent attached
        if (!entity.Has<SpriteComponent>())
        {
            throw new InvalidOperationException("Entity must have a SpriteComponent when loading texture.");
        }

        var spriteComponent = entity.Get<SpriteComponent>();

        if (path == null)
        {
            throw new ArgumentNullException(nameof(path), "Path must be specified when loading texture for an entity.");
        }

        if (!loadedTextures.ContainsKey(path))
        {
            var loadedTexture = contentManager.Load<Texture2D>(path);
            loadedTextures[path] = loadedTexture;
            spriteComponent.Texture = loadedTexture;
        }

        if (entity.Has<AnimationComponent>())
        {
            var animationComponent = entity.Get<AnimationComponent>();
            LoadAnimation(animationComponent);
        }

        return spriteComponent.Texture;
    }

    private void LoadMenuEntity(Entity menuEntity)
    {
        var menuComponent = menuEntity.Get<MenuComponent>();

        if (!loadedTextures.ContainsKey(menuComponent.Texture.Name))
        {
            // Declare the SpriteComponent variable outside the if block
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
            var loadedTexture = contentManager.Load<Texture2D>(menuComponent.Texture.Name);
            loadedTextures[menuComponent.Texture.Name] = loadedTexture;

            // Make sure the SpriteComponent's Texture property is updated
            spriteComponent.Texture = loadedTexture;
        }
    }

    private void LoadAnimation(AnimationComponent animationComponent)
    {
        animationComponent.SpriteSheet = contentManager.Load<SpriteSheet>(animationComponent.PathToSF, new JsonContentLoader());
        animationComponent.AnimatedSprite = new AnimatedSprite(animationComponent.SpriteSheet);
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
    }
}
