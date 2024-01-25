
using MonoGame.Extended.Content;
using System;

namespace Fizzleon.Core;
public class GameContentLoader : ContentManager
{
    public GameContentLoader(IServiceProvider serviceProvider, string rootDirectory)
             : base(serviceProvider, rootDirectory)
    {
    }
}
