﻿using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using static Fizzleon.Core.Data.GameState;
using Fizzleon.Scenes;  // Add reference to your scene namespace
using System;
using MonoGame.Extended.Entities.Systems;
using Fizzleon.Managers;

namespace Fizzleon.ECS.Systems
{
    public class TransitionSystem : EntityUpdateSystem
    {
        private ComponentMapper<SceneTransitionComponent> transitionMapper;
        private SceneManager sceneManager;

        public TransitionSystem(SceneManager sceneManager) : base(Aspect.All(typeof(SceneTransitionComponent)))
        {
            this.sceneManager = sceneManager ?? throw new ArgumentNullException(nameof(sceneManager));
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transitionMapper = mapperService.GetMapper<SceneTransitionComponent>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                var transitionComponent = transitionMapper.Get(entity);

                switch (transitionComponent.CurrentTransitionState)
                {
                    case SceneTransitionComponent.TransitionState.TransitionIn:
                        transitionComponent.TransitionIn();
                        break;

                    case SceneTransitionComponent.TransitionState.TransitionOut:
                        transitionComponent.TransitionOut();
                        break;

                    default:
                        break;
                }
            }

            // Access the SceneManager instance and perform any necessary scene changes
            if (sceneManager != null)
            {
                // Example: sceneManager.ChangeScene(newScene);
            }
        }
    }
}
