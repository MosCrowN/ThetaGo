using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

internal class ButtonLayout: Drawable
{
    public Sprite[]? Buttons;

    public RectangleShape?[]? Sliders;

    private int _selected = -1;

    public int Selected(int x, int y, bool isPressed)
    {
        if (Buttons == null) return -1;
        for (var i = 0; i < Buttons.Length; ++i)
        {
            if (x < Buttons[i].Position.X || x > (Buttons[i].Position.X + Buttons[i].Texture.Size.X * Buttons[i].Scale.X) ||
                y < Buttons[i].Position.Y || y > (Buttons[i].Position.Y + Buttons[i].Texture.Size.Y * Buttons[i].Scale.Y)) 
                continue;
            _selected = i;
            if (_selected == -1 || Sliders?[_selected] == null || !isPressed) return _selected;
            var scale = new Vector2f((x - Sliders[_selected]!.Position.X) / Sliders[_selected]!.Size.X, 1);
            scale.X = scale.X < 0 ? 0 : (scale.X > 1 ? 1 : scale.X);
            Sliders![_selected]!.Scale = scale;
        }

        return _selected;
    }

    public float Slider
    {
        get
        {
            if (_selected < 0 || Sliders?[_selected] == null)
                return -1;
            return Sliders[_selected]!.Scale.X;
        }
    }

    //TODO: scrolling

    public void Draw(RenderTarget target, RenderStates states)
    {
        //TODO: paint the pressed button and make noise
        if (Buttons == null) return;
        foreach (var i in Buttons)
            target.Draw(i);
        if (Sliders == null) return;
        foreach (var i in Sliders)
            if (i != null) target.Draw(i);
    }
}

internal static class LayoutFactory
{
    public static ButtonLayout MainMenu(float x, float y)
    {
        var layout = new ButtonLayout
        {
            Buttons = new Sprite[4],
            Sliders = new RectangleShape?[4]
        };

        layout.Sliders[0] = new RectangleShape
        { 
            Size = new Vector2f(x * 0.5f, 3), 
            FillColor = Color.Green, 
            Position = new Vector2f(0.25f * x, y / 6f)
        };

        for (var i = 0; i < 4; ++i)
        {
            layout.Buttons[i] = new Sprite
            {
                Texture = Params.MenuButton,
                Scale = new Vector2f((0.5f * x) / Params.MenuButton.Size.X,
                    (0.15f * y) / Params.MenuButton.Size.Y),
                Position = new Vector2f(x * 0.25f, y * (i + 1) / 6f)
            };
        }

        return layout;
    }

    public static ButtonLayout Settings(float x, float y)
    {
        var layout = new ButtonLayout
        {
            Buttons = new Sprite[4],
        };

        return layout;
    }
}