
public interface IComponent
{
    public abstract void LoadContent(ContentManager Content);
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);

}
