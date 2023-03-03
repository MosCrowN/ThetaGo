using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

class Board : Drawable
{
    private int _size, _step;

    private Sprite _sprite, _black, _white;

    private RectangleShape _lineH, _lineV;

    public States[,] Desk;

    private (int ix, int iy) _select;

    public (int ix, int iy) Selected
    {
        set
        {
            _select.ix = (int)Math.Round((value.ix - _sprite.Position.X) / _step);
            _select.iy = (int)Math.Round((value.iy - _sprite.Position.Y) / _step);
            
            _lineH.Position = new Vector2f(0, _select.iy * _step);
            _lineV.Position = new Vector2f(_select.ix * _step, 0);
            _lineH.Position += _sprite.Position;
            _lineV.Position += _sprite.Position;
        }
        //get => _select;
    }

    //TODO: use screen sizes
    public Board(int screenX, int screenY)
    {
        _size = Settings.DeskSize - 1;

        var min = Math.Min(screenX, screenY);
        var size = (int)(min * 5 / 6f);
        _step = size / _size;
        //TODO: add desk boarders
        var texture = new RenderTexture((uint)size + 3, (uint)size + 3);
        //TODO: set background picture
        texture.Clear(Color.Yellow);

        _lineH = new RectangleShape();
        _lineV = new RectangleShape();
        _lineH.Size = new Vector2f((uint)(_step * _size + 3), 3);
        _lineV.Size = new Vector2f(3, (uint)(_step * _size + 3));
        _lineH.FillColor = Color.Black;
        _lineV.FillColor = Color.Black;

        _size += 3;
        Desk = new States[_size, _size];
        for (var i = 0; i < _size; ++i)
        {
            _lineH.Position = new Vector2f(0, i * _step);
            _lineV.Position = new Vector2f(i * _step, 0);
            texture.Draw(_lineH);
            texture.Draw(_lineV);

            for (var j = 0; j < _size; ++j)
                Desk[i, j] = States.Free;

            Desk[i, 0] = States.Edge;
            Desk[i, _size - 1] = States.Edge;
            Desk[0, i] = States.Edge;
            Desk[_size - 1, i] = States.Edge;
        }

        texture.Display();
        _sprite = new Sprite(texture.Texture);
        _sprite.Position = new Vector2f((min - size) / 2f, (min - size) / 2f);
        
        _black = new Sprite(Settings.BlackStone);
        _black.Scale = new Vector2f((float)_step / Settings.BlackStone.Size.X,
            (float)_step / Settings.BlackStone.Size.Y);
        _white = new Sprite(Settings.WhiteStone);
        _white.Scale = new Vector2f((float)_step / Settings.WhiteStone.Size.X,
            (float)_step / Settings.WhiteStone.Size.Y);
        
        _lineH.FillColor = Color.White;
        _lineV.FillColor = Color.White;
    }

    private static bool _isWhite { set; get; }
    
    public void PutStone(int ix = -1, int iy = -1)
    {
        //Console.WriteLine(_select);
        if (ix == -1 || iy == -1) (ix, iy) = _select;
        
        if (_isWhite) Desk[ix + 1, iy + 1] = States.White;
        else Desk[ix + 1, iy + 1] = States.Black;
        _isWhite = !_isWhite;

        //TODO: check Ko rule
        //TODO: check if the stone surrounded
        //TODO: paint previous stone
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(_sprite);
        target.Draw(_lineH);
        target.Draw(_lineV);

        var size = _size - 1;
        for (var ix = 0; ix < size; ++ix)
        for (var iy = 0; iy < size; ++iy)
        {
            //TODO: remove shift
            var shift = new Vector2f(2f, 2f);
            if (Desk[ix + 1, iy + 1] == States.White)
            {
                _white.Position = new Vector2f(_sprite.Position.X - _step / 2f + _step * ix,
                    _sprite.Position.Y - _step / 2f + _step * iy);
                _white.Position += shift;
                target.Draw(_white);
            }
            else if (Desk[ix + 1, iy + 1] == States.Black)
            {
                _black.Position = new Vector2f(_sprite.Position.X - _step / 2f + _step * ix,
                    _sprite.Position.Y - _step / 2f + _step * iy);
                _black.Position += shift;
                target.Draw(_black);
            }
        }
    }

    public enum States : sbyte
    {
        White = 1,
        Black = -1,
        Free = 0,
        Edge = 2,
    }
}

class BoardSprite : Drawable
{
    
}