/*
using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

internal class Button : Drawable
{
    public Sprite? S;

    public RectangleShape? R;

    public Text? T;

    //TODO: paint the pressed button and make noise
    //public bool IsActivated;
    public void UpdTxt(int value)
    {
        var str = string.Empty;
        foreach (var i in T!.DisplayedString)
        {
            str += i;
            if (i != ':') continue;
            T!.DisplayedString = str + ' ' + value;
            return;
        }
    }
    
    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(S);
        if (R != null)
            target.Draw(R);
        if (T != null)
            target.Draw(T);
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
            var lx = (int)(x - Buttons[i].S!.Position.X);
            var ly = (int)(y - Buttons[i].S!.Position.Y);
            if (lx < 0 || lx > Buttons[i].S!.GetGlobalBounds().Width ||
                ly < 0 || ly > Buttons[i].S!.GetGlobalBounds().Height) 
                continue;
            _selected = i;
            //TODO: change the button text
            if (Buttons[_selected].R == null || !isPressed) return _selected;
            var scale = new Vector2f(lx / Buttons[_selected].R!.Size.X, 1);
            scale.X = scale.X < 0 ? 0 : (scale.X > 1 ? 1 : scale.X);
            Buttons[_selected].R!.Scale = scale;
        }

        return _selected;
    }

    public int Slider
    {
        get
        {
            if (_selected < 0 || Buttons?[_selected].R! == null)
                return -1;
            return (int)(100 * Buttons[_selected].R!.Scale.X);
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
    private static string _mainMenu(int n)
    {
        return n switch
        {
            0 => "CONTINUE",
            1 => "NEW GAME",
            2 => "SETTINGS",
            3 => "EXIT",
            _ => ""
        };
    }
    //TODO: centralize the text
    public static ButtonLayout MainMenu(float x, float y)
    {
        const int n = 4;
        var layout = new ButtonLayout
        {
            Buttons = new Button[n]
        };

        for (var i = 0; i < n; ++i)
        {
            layout.Buttons[i] = new Button
            {
                S = new Sprite
                {
                    Texture = Params.MenuButton,
                    Scale = new Vector2f((0.5f * x) / Params.MenuButton.Size.X,
                        (0.15f * y) / Params.MenuButton.Size.Y),
                    Position = new Vector2f(x * 0.25f, y * (i + 1) / 6f)
                },
                T = new Text
                {
                    Font = Params.Albert,
                    FillColor = Color.Magenta,
                    CharacterSize = 500,
                    DisplayedString = _mainMenu(i)
                }
            };
            
            var size = new Vector2f(layout.Buttons[i].S!.GetGlobalBounds().Width, 
                layout.Buttons[i].S!.GetGlobalBounds().Height);
            var oldTextSize = new Vector2f(layout.Buttons[i].T!.GetGlobalBounds().Width,
                layout.Buttons[i].T!.GetGlobalBounds().Height);
            var newTextSize = new Vector2f(layout.Buttons[i].T!.DisplayedString.Length * (size.X / 25f), 0.33f * size.Y);
            layout.Buttons[i].T!.Scale = new Vector2f(newTextSize.X / oldTextSize.X, newTextSize.Y / oldTextSize.Y);
            layout.Buttons[i].T!.Position = layout.Buttons[i].S!.Position + (size - newTextSize) / 2f;
        }

        return layout;
    }
    
    private static string _settings(int n)
    {
        return n switch
        {
            0 => "MUSIC VOLUME: 100",
            1 => "DESK SIZE: 19",
            2 => "DIFFICULTY: 10",
            3 => "MULTIPLAYER",
            4 => "SAVE & BACK",
            _ => ""
        };
    }

    public static ButtonLayout Settings(float x, float y)
    {
        const int n = 5, n1 = 3;

        var layout = new ButtonLayout
        {
            Buttons = new Button[n]
        };

        for (var i = 0; i < n; ++i)
        {
            layout.Buttons[i] = new Button
            {
                S = new Sprite
                {
                    Texture = Params.MenuButton,
                    Scale = new Vector2f((0.35f * x) / Params.MenuButton.Size.X,
                        (0.125f * y) / Params.MenuButton.Size.Y),
                    Position = new Vector2f(x * (1 - 0.35f) / 2, y * (i + 1) / 7f)
                },
                T = new Text
                {
                    Font = Params.Albert,
                    FillColor = Color.Magenta,
                    CharacterSize = 500,
                    DisplayedString = _settings(i)
                }
            };
            
            var size = new Vector2f(layout.Buttons[i].S!.GetGlobalBounds().Width, 
                layout.Buttons[i].S!.GetGlobalBounds().Height);
            var oldTextSize = new Vector2f(layout.Buttons[i].T!.GetGlobalBounds().Width,
                layout.Buttons[i].T!.GetGlobalBounds().Height);
            var newTextSize = new Vector2f(layout.Buttons[i].T!.DisplayedString.Length * (size.X / 25f), 0.33f * size.Y);
            layout.Buttons[i].T!.Scale = new Vector2f(newTextSize.X / oldTextSize.X, newTextSize.Y / oldTextSize.Y);
            layout.Buttons[i].T!.Position = layout.Buttons[i].S!.Position + (size - newTextSize) / 2f;
        }

        for (var i = 0; i < n1; ++i)
            layout.Buttons[i].R = new RectangleShape
            {
                Size = new Vector2f(x * 0.35f, 3),
                FillColor = Color.Green,
                Position = new Vector2f(layout.Buttons[i].S!.Position.X,
                    layout.Buttons[i].S!.Position.Y + 0.0625f * y)
            };

        layout.Buttons[0].R!.Scale = new Vector2f(Params.MusicVolume / 100f, 1);
        layout.Buttons[1].R!.Scale = new Vector2f(Params.DeskSize / 35f, 1);

        return layout;
    }
}
*/