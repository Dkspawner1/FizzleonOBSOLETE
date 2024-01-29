using System;
using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;

namespace Fizzleon.ECS.Entities;

public class ButtonEntity : IDisposable
{
    public ButtonEntity(Entity parent, Texture2D texture, Rectangle rectangle, World world)
    {
        try
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent), "Parent entity cannot be null.");
            }

            if (world == null)
            {
                Trace.WriteLine("Error: World is null.");
                throw new InvalidOperationException("World is null. Ensure that the entity is attached to a valid World.");
            }

            SceneEntity = world.CreateEntity();

            if (SceneEntity == null)
            {
                Trace.WriteLine("Error: SceneEntity is null.");
                throw new InvalidOperationException("SceneEntity is null.");
            }

            // Create and attach TransformComponent
            var transformComponent = new TransformComponent(rectangle.Location.ToVector2());
            SceneEntity.Attach(transformComponent);

            // Create and attach SpriteComponent
            var spriteComponent = new SpriteComponent(texture);
            spriteComponent.Transform = transformComponent;

            SceneEntity.Attach(spriteComponent);

            // Store the components for later reference if needed
            TransformComponent = transformComponent;
            SpriteComponent = spriteComponent;
            Rectangle = rectangle;
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Error creating ButtonEntity: {ex}");
            throw;
        }
    }
    public TransformComponent TransformComponent { get; private set; }
    public SpriteComponent SpriteComponent { get; private set; }
    public Rectangle Rectangle { get; }
    public Entity SceneEntity { get; }
    public void Dispose()
    {
    }
}