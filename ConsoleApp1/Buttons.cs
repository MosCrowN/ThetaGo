using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

internal class Button : Drawable
{
    public Sprite? S;

    public RectangleShape? R;

    //TODO: paint the pressed button and make noise
    //public bool IsActivated;
    
    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(S);
        if (R != null)
            target.Draw(R);
    }
}

internal class ButtonLayout: Drawable
{
    public Button[]? Buttons;

    private int _selected = -1;

    public int Selected(int x, int y, bool isPressed)
    {
        _selected = -1;
        if (Buttons == null) return -1;
        
        for (var i = 0; i < Buttons.Length; ++i)
        {
            if (x < Buttons[i].S!.Position.X || x > (Buttons[i].S!.Position.X + Buttons[i].S!.Texture.Size.X * Buttons[i].S!.Scale.X) ||
                y < Buttons[i].S!.Position.Y || y > (Buttons[i].S!.Position.Y + Buttons[i].S!.Texture.Size.Y * Buttons[i].S!.Scale.Y)) 
                continue;
            _selected = i;
            if (Buttons[_selected].R == null || !isPressed) return _selected;
            var scale = new Vector2f((x - Buttons[_selected].R!.Position.X) / Buttons[_selected].R!.Size.X, 1);
            scale.X = scale.X < 0 ? 0 : (scale.X > 1 ? 1 : scale.X);
            Buttons[_selected].R!.Scale = scale;
        }

        return _selected;
    }

    public float Slider
    {
        get
        {
            if (_selected < 0 || Buttons?[_selected].R! == null)
                return -1;
            return Buttons[_selected].R!.Scale.X;
        }
    }

    //TODO: scrolling

    public void Draw(RenderTarget target, RenderStates states)
    {
        if (Buttons == null) return;
        foreach (var i in Buttons)
            target.Draw(i);
    }
}

internal static class LayoutFactory
{
    public static ButtonLayout MainMenu(float x, float y)
    {
        const int n = 4;
        var layout = new ButtonLayout
        {
            Buttons = new Button[n]
        };

        for (var i = 0; i < n; ++i)
            layout.Buttons[i] = new Button
            {
                S = new Sprite
                {
                    Texture = Params.MenuButton,
                    Scale = new Vector2f((0.5f * x) / Params.MenuButton.Size.X,
                        (0.15f * y) / Params.MenuButton.Size.Y),
                    Position = new Vector2f(x * 0.25f, y * (i + 1) / 6f)
                }
            };

        return layout;
    }

    public static ButtonLayout Settings(float x, float y)
    {
        const int n = 5, n1 = 3;

        var layout = new ButtonLayout
        {
            Buttons = new Button[n]
        };

        for (var i = 0; i < n; ++i)
            layout.Buttons[i] = new Button
            {
                S = new Sprite
                {
                    Texture = Params.MenuButton,
                    Scale = new Vector2f((0.35f * x) / Params.MenuButton.Size.X,
                        (0.125f * y) / Params.MenuButton.Size.Y),
                    Position = new Vector2f(x * (1 - 0.35f) / 2, y * (i + 1) / 7f)
                }
            };

        for (var i = 0; i < n1; ++i)
            layout.Buttons[i].R = new RectangleShape
            {
                Size = new Vector2f(x * 0.35f, 3),
                FillColor = Color.Green,
                Position = new Vector2f(layout.Buttons[i].S!.Position.X,
                    layout.Buttons[i].S!.Position.Y + 0.0625f * y)
            };

        return layout;
    }
}