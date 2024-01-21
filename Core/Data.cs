using MonoGame.Extended.Entities;

namespace Fizzleon.Core;

public record Data
{
    public readonly record struct Window
    {
        public static string Title { get; set; } = "FizzleOn!";
        public static int Width { get; set; } = 1600;
        public static int Height { get; set; } = 900;
        public static bool Exit { get; set; } = false;
    };

    public readonly record struct Game
    {
        public static GraphicsDeviceManager Graphics { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static World World { get; set; }
        public static WorldBuilder WorldBuilder { get; set; }
    };

    public readonly record struct GameState
    {
        public enum GameStates { MENU, GAME, SETTINGS, EXIT }
        //public static GameStates CurrentState { get; set; } = GameStates.MENU;
    }

}