using System;

public class MenuComponent : IDisposable
{
    public Texture2D Texture { get; set; }
    public Rectangle Rectangle { get; set; }

    public MenuComponent(Texture2D texture, Rectangle rectangle)
    {
        Texture = texture;
        Rectangle = rectangle;
    }

    public void Dispose()
    {
        Texture?.Dispose();
    }
}
