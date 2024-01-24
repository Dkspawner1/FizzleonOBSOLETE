// SceneBase.cs

using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using static Fizzleon.Core.Data;
using static Fizzleon.ECS.Components.SceneTransitionComponent;

namespace Fizzleon.Core
{
    /// <summary>
    /// Base class for scenes in the game.
    /// </summary>
    public abstract class SceneBase : IScene
    {
        protected World world;
        public World World
        {
            get => world;
            set => world = value;
        }

        public abstract GameState.GameStates SceneId { get; }
        public abstract bool IsSceneChangeRequested { get; set; }
        public abstract TransitionState CurrentTransitionState { get; }

        /// <summary>
        /// Gets the entity representing the scene.
        /// </summary>
        public virtual Entity? SceneEntity => null;

        /// <summary>
        /// Gets the transition component associated with the scene.
        /// </summary>
        public virtual SceneTransitionComponent? TransitionComponent => null;

        /// <summary>
        /// Loads content for the scene.
        /// </summary>
        public virtual void LoadContent()
        {
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Updates the scene.
        /// </summary>
        public virtual void Update()
        {
            world?.Update(Data.GameTime);
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        public virtual void Draw()
        {
            world?.Draw(Data.GameTime);
        }

        /// <summary>
        /// Disposes of the scene.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Transitions the scene in.
        /// </summary>
        public abstract void TransitionIn();

        /// <summary>
        /// Transitions the scene out.
        /// </summary>
        public abstract void TransitionOut();

        /// <summary>
        /// Initializes the world with the specified systems.
        /// </summary>
        protected void InitializeWorld(params ISystem[] systems)
        {
            var worldBuilder = new WorldBuilder();

            foreach (var system in systems)
            {
                worldBuilder.AddSystem(system);
            }

            world = worldBuilder.Build();
        }
    }
}
