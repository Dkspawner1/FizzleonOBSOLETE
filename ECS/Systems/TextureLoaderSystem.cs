using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System.Collections.Generic;
using MonoGame.Extended.Content;

internal class TextureLoaderSystem : ISystem
{
    private readonly ContentManager contentManager;
    private readonly Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();

    public TextureLoaderSystem(ContentManager contentManager)
    {
        this.contentManager = contentManager;
    }

    public void LoadEntities(IEnumerable<Entity> entities)
    {
        foreach (var entity in entities)
        {
            LoadTexture(entity);
        }
    }

    public void LoadMenuItems(List<Texture2D> menuItems, List<Rectangle> menuItemRectangles)
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            LoadMenuItemTexture(menuItems[i], menuItemRectangles[i]);
        }
    }

    public void LoadTextures(IEnumerable<Entity> entities, List<Texture2D> menuItems, List<Rectangle> menuItemRectangles)
    {
        // Load textures for entities
        foreach (var entity in entities)
        {
            LoadTexture(entity);
        }

        // Load textures for menu items
        LoadMenuItems(menuItems, menuItemRectangles);
    }

    private void LoadMenuItemTexture(Texture2D menuItemTexture, Rectangle menuItemRectangle)
    {
 
    }

    private void LoadTexture(Entity entity)
    {
        var spriteComponent = entity.Get<SpriteComponent>();

        if (!loadedTextures.ContainsKey(spriteComponent.Texture.Name))
        {
            var loadedTexture = contentManager.Load<Texture2D>(spriteComponent.Texture.Name);
            loadedTextures[spriteComponent.Texture.Name] = loadedTexture;

            spriteComponent.Texture = loadedTexture;
        }

        if (entity.Has<AnimationComponent>())
        {
            var animationComponent = entity.Get<AnimationComponent>();
            LoadAnimation(animationComponent);
        }
    }

    private void LoadAnimation(AnimationComponent animationComponent)
    {
        // Load AnimationComponent-specific components
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
