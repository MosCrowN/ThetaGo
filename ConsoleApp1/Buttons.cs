using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

class Layout: Drawable
{
    public Sprite[]? Buttons;
    
    public int[][]? ButtonBoarders;
    
    public RectangleShape?[]? Sliders;

    public int[]? SliderLevels;

    private int _selected = -1;

    private int _x, _y;

    private (int dx, int dy) _slide;
    public (int dx, int dy) Slide
    {
        private set
        {
            _slide.dx = value.dx;
            _slide.dy = value.dy;
            
            if (_selected == -1 || Sliders == null ||
                _selected >= Sliders.Length || Sliders[_selected] == null)
                return;

            if (SliderLevels == null) return;
            var scale = Sliders[_selected]!.Scale + 
                        new Vector2f(1 - (SliderLevels[_selected] - value.dx) /
                            (float)SliderLevels[_selected], 0);
            if (scale.X < 0) scale = new Vector2f(0, 1);
            else if (scale.X > 1) scale = new Vector2f(1, 1);
            Sliders[_selected]!.Scale = scale;
        }
        get => _slide;
    }

    public int Selected(int x, int y)
    {
        Slide = (x - _x, y - _y);
        _x = x;
        _y = y;
        
        if (Buttons == null || ButtonBoarders == null) return -1;
        for (var i = 0; i < Buttons.Length; ++i)
        {
            if (x > ButtonBoarders[i][0] && x < ButtonBoarders[i][2] &&
                y > ButtonBoarders[i][1] && y < ButtonBoarders[i][3])
                return _selected = i;
        }

        return _selected = -1;
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
            target.Draw(i);
    }
}

static class ButtonFactory
{
    public static Layout MainMenu(float x, float y)
    {
        var layout = new Layout
        {
            Buttons = new Sprite[4],
            ButtonBoarders = new int[4][]
        };

        layout.Sliders = new[]
        {
            new RectangleShape
            {
                Size = new Vector2f(x * 0.5f, 3),
                FillColor = Color.Green,
                Position = new Vector2f(0.25f * x, y / 6f)
            }
        };

        layout.SliderLevels = new[]
        {
            (int)(x * 0.5f)
        };

        for (var i = 0; i < 4; ++i)
        {
            var x0 = (int)(x * 0.25f);
            var y0 = (int)(y * (i + 1) / 6f);
            var x1 = (int)(0.5f * x);
            var y1 = (int)(0.15f * y);
            
            layout.Buttons[i] = new Sprite
            {
                Texture = Settings.MenuButton,
                Scale = new Vector2f((float)x1 / Settings.MenuButton.Size.X,
                    (float)y1 / Settings.MenuButton.Size.Y),
                Position = new Vector2f(x0, y0)
            };
            layout.ButtonBoarders[i] = new[] { x0, y0, x0 + x1, y0 + y1};
        }

        return layout;
    }

    public static Layout Setting(float x, float y)
    {
        var layout = new Layout
        {
            Buttons = new Sprite[4],
            ButtonBoarders = new int[4][]
        };

        return layout;
    }
}