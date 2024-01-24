using MonoGame.Extended.Input.InputListeners;

namespace Fizzleon.Core;

public sealed record Data
{
    public static ContentManager Content { get; set; }
    public static GameTime GameTime { get; set; }
    public static GraphicsDeviceManager Graphics { get; set; }
    public static SpriteBatch SpriteBatch { get; set; }
    public static MouseListener MouseListener;

    public readonly record struct Window
    {
        public static string Title { get; set; } = "FizzleOn!";
        public static int Width { get; set; } = 1600;
        public static int Height { get; set; } = 900;
        public static bool Exit { get; set; } = false;
    };

   

    public readonly record struct GameState
    {
        public enum GameStates { MENU, GAME, SETTINGS, EXIT }
    }

}