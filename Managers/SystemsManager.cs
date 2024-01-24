using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Entities;
using System.Collections.Generic;
using System;

public class SystemsManager : List<ISystem>, IGameComponent
{
    private World world;
    private readonly WorldBuilder worldBuilder;
    private Game1 game;

    public SystemsManager(Game1 game, ContentManager contentManager)
    {
        this.game = game;
        this.worldBuilder = new WorldBuilder();
        InitializeSystems(contentManager);  // Pass contentManager to InitializeSystems
    }
    public World GetWorld() => world;

    public void Initialize()
    {
        // Initialize the systems
        foreach (var system in this)
        {
            (system as IUpdateSystem)?.Initialize(world);
        }

        // Build the world with all systems
        world = worldBuilder.Build();
    }

    public void Update(GameTime gameTime)
    {
        // Ensure the world is not null
        if (world != null)
        {
            // Update the systems manually
            foreach (var system in this)
            {
                if (system is IUpdateSystem updateSystem)
                {
                    updateSystem.Update(gameTime);
                }
            }
        }
    }

    public void Dispose()
    {
        if (world == null)
            return;

        // Dispose of the systems
        foreach (var system in this)
        {
            (system as IDisposable)?.Dispose();
        }

        Clear();
    }

    private void InitializeSystems(ContentManager contentManager)
    {
        var textureLoaderSystem = new TextureLoaderSystem(contentManager);
        Add(textureLoaderSystem);
        worldBuilder.AddSystem(textureLoaderSystem);

        // Pass contentManager to Initialize method
        textureLoaderSystem.Initialize(world);
    }
}
