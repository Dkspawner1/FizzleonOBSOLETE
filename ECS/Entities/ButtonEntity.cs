using MonoGame.Extended.Entities;
using System;

public class ButtonEntity : IDisposable
{
    public Texture2D Texture { get; set; }
    public Rectangle Rectangle { get; set; }
    public Entity SceneEntity { get; set; } 
    public ButtonEntity(Entity sceneEntity, Texture2D texture, Rectangle rectangle)
    {
        SceneEntity = sceneEntity;
        Texture = texture;
        Rectangle = rectangle;
    }

    public void Dispose()
    {
    }
}