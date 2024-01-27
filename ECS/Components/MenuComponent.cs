using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using System;

public class MenuComponent : IDisposable
{
    public Texture2D Texture { get; set; }
    public Rectangle Rectangle { get; set; }
    public Entity Entity { get; set; }  // Add this property

    public MenuComponent(Texture2D texture, Rectangle rectangle, Entity entity)
    {
        Texture = texture;
        Rectangle = rectangle;
        Entity = entity;  
    }

    public void Dispose()
    {
        Texture?.Dispose();
    }
}