using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

internal class Button : Drawable
{
    public Sprite S;

    public RectangleShape? R;

    public Text? T;

    public int MaxLevel, CurLevel;

    //TODO: paint the pressed button and make noise
    //public bool IsActivated;

    public Button()
    {
        S = new Sprite
        {
            Texture = Params.MenuButton,
        };
        T = new Text
        {
            Font = Params.Albert,
            FillColor = Color.Magenta,
            CharacterSize = 500
        };
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(S);
        
        if (T == null) return;
        
        if (R != null)
        {
            target.Draw(R);
            var str = string.Empty;
            foreach (var i in T!.DisplayedString)
            {
                str += i;
                if (i != ':') continue;
                CurLevel = (int)(MaxLevel * R.Scale.X);
                T!.DisplayedString = str + ' ' + CurLevel;
                break;
            }
        }

        target.Draw(T);
    }
}

internal class ButtonLayout: Drawable
{
    private List<Button> _buttons = new ();

    private int _selected = -1;

    public int Selected(int x, int y, bool isPressed)
    {
        _selected = -1;

        for (var i = 0; i < _buttons.Count; ++i)
        {
            var lx = (int)(x - _buttons[i].S!.Position.X);
            var ly = (int)(y - _buttons[i].S!.Position.Y);
            if (lx < 0 || lx > _buttons[i].S!.GetGlobalBounds().Width ||
                ly < 0 || ly > _buttons[i].S!.GetGlobalBounds().Height) 
                continue;
            _selected = i;
            //TODO: change the button text
            if (_buttons[_selected].R == null || !isPressed) return _selected;
            var scale = new Vector2f(lx / _buttons[_selected].R!.Size.X, 1);
            scale.X = scale.X < 0 ? 0 : (scale.X > 1 ? 1 : scale.X);
            _buttons[_selected].R!.Scale = scale;
        }

        return _selected;
    }

    public int Slider
    {
        get
        {
            if (_selected < 0 || _buttons[_selected].R == null)
                return -1;
            return _buttons[_selected].CurLevel;
        }
    }

    //TODO: scrolling

    public void Draw(RenderTarget target, RenderStates states)
    {
        foreach (var i in _buttons)
            target.Draw(i);
    }
    
    public void Compile(int x0, int x1, int y0, int y1)
    {
        for (var i = 0; i < _buttons.Count; ++i)
        {
            _buttons[i].S.Scale = new Vector2f((float)(x1 - x0) / _buttons[i].S.Texture.Size.X,
                (float)(y1 - y0) / (_buttons.Count + 1) / _buttons[i].S.Texture.Size.Y);
            _buttons[i].S.Position = new Vector2f(x0, y0 + (y1 - y0) / (_buttons.Count + 1) * i);
            if (_buttons[i].T == null) continue;
            var size = new Vector2f(_buttons[i].S.GetGlobalBounds().Width,
                _buttons[i].S.GetGlobalBounds().Height);
            var oldTextSize = new Vector2f(_buttons[i].T!.GetGlobalBounds().Width,
                _buttons[i].T!.GetGlobalBounds().Height);
            var newTextSize = new Vector2f(_buttons[i].T!.DisplayedString.Length * (size.X / 25f), 0.33f * size.Y);
            _buttons[i].T!.Scale = new Vector2f(newTextSize.X / oldTextSize.X, newTextSize.Y / oldTextSize.Y);
            _buttons[i].T!.Position = _buttons[i].S.Position + (size - newTextSize) / 2f;
            if (_buttons[i].R == null) continue;
            _buttons[i].R!.Size = new Vector2f(size.X, 3);
            _buttons[i].R!.Position = new Vector2f(_buttons[i].S.Position.X,
                _buttons[i].S.Position.Y + size.Y / 2);
        }
    }

    public void Add(string text, Texture? texture = null)
    {
        var button = new Button();
        if (texture != null)
            button.S.Texture = texture;
        button.T!.DisplayedString = text;
        _buttons.Add(button);
    }

    public void Add(string text, int maxLevel, int curLevel = -1, Texture? texture = null)
    {
        var button = new Button();
        if (texture != null)
            button.S.Texture = texture;
        button.T!.DisplayedString = text;
        button.R = new RectangleShape
        {
            FillColor = Color.Green,
            Scale = curLevel > 0 ? new Vector2f
                ((float)curLevel / maxLevel, 1) : new Vector2f(1, 1)
        };
        button.MaxLevel = maxLevel;
        _buttons.Add(button);
    }
}