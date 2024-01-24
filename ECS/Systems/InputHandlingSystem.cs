using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Input;
using System.Collections.Generic;

internal class InputHandlingSystem : UpdateSystem
{
    private readonly MouseListener mouseListener;
    private readonly List<Rectangle> buttonsRect;

    public InputHandlingSystem(MouseListener mouseListener, List<Rectangle> buttonsRect)
    {
        this.mouseListener = mouseListener;
        this.buttonsRect = buttonsRect;

        // Subscribe to the MouseClicked event
        mouseListener.MouseClicked += OnMouseClick;
    }

    public override void Update(GameTime gameTime)
    {
        mouseListener.Update(gameTime);
    }

    private void OnMouseClick(object sender, MouseEventArgs e)
    {
        for (int i = 0; i < buttonsRect.Count; i++)
        {
            if (buttonsRect[i].Contains(e.Position) && e.Button == MouseButton.Left)
            {
                // Button i is clicked
                HandleButtonClick(i);
            }
        }
    }

    private void HandleButtonClick(int buttonIndex)
    {
        // Handle the button click based on the index
        if (buttonIndex == 0)
        {
            // Handle button 0 click
            // Example: IsSceneChangeRequested = true;
        }
        else if (buttonIndex == 2)
        {
            // Handle button 2 click
            // Example: Window.Exit = true;
        }
    }
}
