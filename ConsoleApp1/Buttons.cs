using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

class Layout: Drawable
{
    public Sprite[]? Buttons { set; get; }
    public int[][]? ButtonBoarders { set; get; }
    public RectangleShape[]? Sliders { set; get; }
    
    public int[]? SliderLevels { set; get; }

    private int _selected = -1;

    private int _x, _y;

    private (int dx, int dy) _slide;
    public (int dx, int dy) Slide
    {
        private set
        {
            _slide.dx = value.dx;
            _slide.dy = value.dy;
            //TODO: change sliders size
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
        var layout = new Layout();
        var texture = new Texture("button.png");
        layout.Buttons = new Sprite[4];
        layout.ButtonBoarders = new int[4][];
        for (var i = 0; i < 4; ++i)
        {
            var x0 = (int)(x * 0.25f);
            var y0 = (int)(y * (i + 1) / 6f);
            var x1 = (int)(0.5f * x);
            var y1 = (int)(0.15f * y);
            layout.Buttons[i] = new Sprite
            {
                Texture = texture,
                Scale = new Vector2f((float)x1 / texture.Size.X,
                    (float)y1 / texture.Size.Y),
                Position = new Vector2f(x0, y0)
            };
            layout.ButtonBoarders[i] = new[] { x0, y0, x0 + x1, y0 + y1};
        }

        return layout;
    }
}