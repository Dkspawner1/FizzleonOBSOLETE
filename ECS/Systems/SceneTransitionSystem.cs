using Fizzleon.ECS.Components;
using Fizzleon.Managers;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Entities;
using System;

public class SceneTransitionSystem : EntityUpdateSystem
{
    private ComponentMapper<SceneTransitionComponent> transitionMapper;
    private SceneManager sceneManager;

    public SceneTransitionSystem(SceneManager sceneManager) : base(Aspect.All(typeof(SceneTransitionComponent)))
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

        Data.SpriteBatch.Begin();

        DrawTransition();

        Data.SpriteBatch.End();

        if (sceneManager != null)
        {

        }
    }

    public void DrawTransition()
    {
        foreach (var entity in ActiveEntities)
        {
            var transitionComponent = transitionMapper.Get(entity);
            transitionComponent.Draw(Data.SpriteBatch);
        }
    }
}
