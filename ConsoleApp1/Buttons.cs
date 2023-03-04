using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

internal class Button : Drawable
{
    public Sprite? S;

    public RectangleShape? R;

    public bool IsSlider;
    
    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(S);
        if (IsSlider)
            target.Draw(R);
    }
}

internal class ButtonLayout: Drawable
{
    public Button[]? Buttons;

    private int _selected = -1;

    public int Selected(int x, int y, bool isPressed)
    {
        if (Buttons == null) return -1;
        _selected = -1;
        for (var i = 0; i < Buttons.Length; ++i)
        {
            if (x < Buttons[i].S!.Position.X || x > (Buttons[i].S!.Position.X + Buttons[i].S!.Texture.Size.X * Buttons[i].S!.Scale.X) ||
                y < Buttons[i].S!.Position.Y || y > (Buttons[i].S!.Position.Y + Buttons[i].S!.Texture.Size.Y * Buttons[i].S!.Scale.Y)) 
                continue;
            _selected = i;
            if (!Buttons[_selected].IsSlider || !isPressed) return _selected;
            var scale = new Vector2f((x - Buttons[_selected].R!.Position.X) / Buttons[_selected].R!.Size.X, 1);
            scale.X = scale.X < 0 ? 0 : (scale.X > 1 ? 1 : scale.X);
            Buttons[_selected].R!.Scale = scale;
        }

        return -1;
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
        //TODO: paint the pressed button and make noise
        if (Buttons == null) return;
        foreach (var i in Buttons)
            target.Draw(i);
    }
}

internal static class LayoutFactory
{
    public static ButtonLayout MainMenu(float x, float y)
    {
        var layout = new ButtonLayout
        {
            Buttons = new Button[4]
        };

        for (var i = 0; i < 4; ++i)
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

        layout.Buttons[0].IsSlider = true;
        layout.Buttons[0].R = new RectangleShape
        { 
            Size = new Vector2f(x * 0.5f, 3), 
            FillColor = Color.Green, 
            Position = new Vector2f(0.25f * x, y / 6f)
        };

        return layout;
    }

    public static ButtonLayout Settings(float x, float y)
    {
        var layout = new ButtonLayout
        {
            Buttons = new Button[4],
            //Sliders = new RectangleShape?[4]
        };

        return layout;
    }
}