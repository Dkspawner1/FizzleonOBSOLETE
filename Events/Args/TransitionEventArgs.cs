using System;
using Fizzleon.ECS.Components;

namespace Fizzleon.Events.Args
{
    public class TransitionEventArgs : EventArgs
    {
        public TransitionEventArgs(
            SceneTransitionComponent.TransitionState currentState,
            SceneTransitionComponent.TransitionState targetState,
            string sceneChangeMessage,
            string previousSceneId,
            string newSceneId)
        {
            CurrentState = currentState;
            TargetState = targetState;
            SceneChangeMessage = sceneChangeMessage;
            PreviousSceneId = previousSceneId;
            NewSceneId = newSceneId;
        }

        public SceneTransitionComponent.TransitionState CurrentState { get; }
        public SceneTransitionComponent.TransitionState TargetState { get; }
        public string SceneChangeMessage { get; }
        public string PreviousSceneId { get; }
        public string NewSceneId { get; }
    }
}