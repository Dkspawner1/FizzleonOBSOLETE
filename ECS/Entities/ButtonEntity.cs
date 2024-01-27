using System;
using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;

namespace Fizzleon.ECS.Entities;

public class ButtonEntity(Entity sceneEntity, Texture2D texture, Rectangle rectangle) : SpriteComponent(texture), IDisposable
{
    public Rectangle Rectangle { get; set; } = rectangle;
    public Entity SceneEntity { get; set; } = sceneEntity;
    public void Dispose()
    {
        
    }
}