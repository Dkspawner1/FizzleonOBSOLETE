using Fizzleon.ECS.Systems;
using Fizzleon.Scenes;
using MonoGame.Extended.Input.InputListeners;

namespace Fizzleon.Core;

public sealed record Data
{
    public static GameTime GameTime { get; set; }
    public static ContentInitializationSystem ContentInitializationSystem { get; set; }
    public static ContentManager ContentManager { get; set; }
        public static GraphicsDeviceManager Graphics { get; set; }
    public static SpriteBatch SpriteBatch { get; set; }
    public static IScene CurrentScene { get; set; }
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