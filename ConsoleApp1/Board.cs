//using System.Diagnostics;
using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

[Serializable]
public class Board
{
    public readonly States[,] Desk;

    private IArbiter _arbiter;

    public Board() : this(new IngRules()) {}
    
    public Board(IArbiter arbiter)
    {
        var size = Params.DeskSize + 2;
        Desk = new States[size, size];
        _desk = new States[size, size];
        
        for (var i = 0; i < size; ++i)
        for (var j = 0; j < size; ++j)
                Desk[i, j] = States.Free;
        for (var i = 0; i < size; ++i)
        {
            Desk[i, 0] = States.Edge;
            Desk[i, size - 1] = States.Edge;
            Desk[0, i] = States.Edge;
            Desk[size - 1, i] = States.Edge;
        }

        _arbiter = arbiter;
    }

    private States _color;

    private bool IsAlive(int x, int y)
    {
        if (Desk[x, y] == States.Free) 
            return true;
        Desk[x, y] = States.Visited;
        if (Desk[x + 1, y] == _color && IsAlive(x + 1, y) ||
            Desk[x + 1, y] == States.Free) 
            return true;
        if (Desk[x - 1, y] == _color && IsAlive(x - 1, y) ||
            Desk[x - 1, y] == States.Free) 
            return true;
        if (Desk[x, y + 1] == _color && IsAlive(x, y + 1) ||
            Desk[x, y + 1] == States.Free) 
            return true;
        return Desk[x, y - 1] == _color && IsAlive(x, y - 1) ||
               Desk[x, y - 1] == States.Free;
    }
    
    private int _capturedWhite;
    
    private void Capture()
    {
        void CaptureGroup(int x, int y)
        {
            if (Desk[x, y] != States.White &&
                Desk[x, y] != States.Black)
                return;

            _color = Desk[x, y];

            var flag = IsAlive(x, y);

            for (var ix = 1; ix <= 19; ++ix)
            for (var iy = 1; iy <= 19; ++iy)
                if (Desk[ix, iy] == States.Visited)
                {
                    if (flag)
                    {
                        Desk[ix, iy] = _color;
                        continue;
                    }

                    Desk[ix, iy] = States.Free;
                    if (_color == States.White)
                        --_capturedWhite;
                    else ++_capturedWhite;
                }
        }
        
        for (var i = 1; i <= Params.DeskSize; ++i)
        for (var j = 1; j <= Params.DeskSize; ++j)
        {
            if (_isWhite && Desk[i, j] == States.Black)
                CaptureGroup(i, j);
            else if (!_isWhite && Desk[i, j] == States.White)
                CaptureGroup(i, j);
        }
        
        for (var i = 1; i <= Params.DeskSize; ++i)
        for (var j = 1; j <= Params.DeskSize; ++j)
        {
            if (!_isWhite && Desk[i, j] == States.Black)
                CaptureGroup(i, j);
            else if (_isWhite && Desk[i, j] == States.White)
                CaptureGroup(i, j);
        }
    }

    private bool _isWhite;
    
    public static (int ix, int iy) Selected;
    
    public void PutStone()
    {
        if (Selected.ix < 0 || Selected.ix > Params.DeskSize ||
            Selected.iy < 0 || Selected.iy > Params.DeskSize)
            return;
        var (ix, iy) = Selected;
        
        if (Desk[ix + 1, iy + 1] != States.Free) return;
        
        if (_isWhite) Desk[ix + 1, iy + 1] = States.White;
        else Desk[ix + 1, iy + 1] = States.Black;

        //TODO: paint previous stone

        if (KoCheck(ix + 1, iy + 1)) return;
        
         Capture();

        _isWhite = !_isWhite;
    }

    private readonly States[,] _desk;
    
    private bool KoCheck(int x, int y)
    {
        for (var i = 0; i < Params.DeskSize + 2; ++i)
        for (var j = 0; j < Params.DeskSize + 2; ++j)
            if (_desk[i, j] != Desk[i, j])
            {
                for (var ix = 0; ix < Params.DeskSize + 2; ++ix)
                for (var iy = 0; iy < Params.DeskSize + 2; ++iy)
                    _desk[ix, iy] = Desk[ix, iy];

                return false;
            }
        
        Desk[x, y] = States.Free;
        return true;
    }

    public enum States : sbyte
    {
        White,
        Black,
        Free,
        Edge,
        Visited
    }
}

internal class BoardSprite : Drawable
{
    private readonly int _step, _shift;
    
    private readonly Sprite _sprite, _black, _white;

    private readonly RectangleShape _lineH, _lineV;

    private readonly Board.States[,] _desk;
    
    public (int ix, int iy) Selected
    {
        set
        {
            var ix = (int)Math.Round((value.ix - _sprite.Position.X - _shift) / _step);
            var iy = (int)Math.Round((value.iy - _sprite.Position.Y - _shift) / _step);
            ix = ix >= Params.DeskSize ? -1 : (ix < 0 ? -1 : ix);  
            iy = iy >= Params.DeskSize ? -1 : (iy < 0 ? -1 : iy);
            Board.Selected = (ix, iy);
        }
        get => Board.Selected;
    }

    public BoardSprite(in Board board, int screenX, int screenY)
    {
        _desk = board.Desk;

        var min = Math.Min(screenX, screenY);
        var size = (int)(min * 5 / 6f);
        _shift = _step = size / (Params.DeskSize - 1);
        var texture = new RenderTexture((uint)(size + 3 + 2 * _shift),
            (uint)(size + 3 + 2 * _shift));
        texture.Clear();

        var bg = new Sprite(Params.DeskBg);
        bg.Scale = new Vector2f((float)texture.Size.X / Params.DeskBg.Size.X,
            (float)texture.Size.Y / Params.DeskBg.Size.Y);
        texture.Draw(bg);

        _lineH = new RectangleShape
        {
            Size = new Vector2f((uint)(_step * (Params.DeskSize - 1) + 3), 3),
            FillColor = Color.Black
        };
        
        _lineV = new RectangleShape
        {
            Size = new Vector2f(3, (uint)(_step * (Params.DeskSize - 1) + 3)),
            FillColor = Color.Black
        };

        for (var i = 0; i < Params.DeskSize; ++i)
        {
            _lineH.Position = new Vector2f(_shift, i * _step + _shift);
            _lineV.Position = new Vector2f(i * _step + _shift, _shift);
            texture.Draw(_lineH);
            texture.Draw(_lineV);
        }

        texture.Display();
        _sprite = new Sprite(texture.Texture);
        _sprite.Position = new Vector2f((min - size) / 2f - _shift, (min - size) / 2f - _shift);

        _black = new Sprite(Params.BlackStone);
        _black.Scale = new Vector2f((float)_step / Params.BlackStone.Size.X,
            (float)_step / Params.BlackStone.Size.Y);
        _white = new Sprite(Params.WhiteStone);
        _white.Scale = new Vector2f((float)_step / Params.WhiteStone.Size.X,
            (float)_step / Params.WhiteStone.Size.Y);

        _lineH.FillColor = Color.White;
        _lineV.FillColor = Color.White;
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(_sprite);
        
        if (Selected is { ix: >= 0, iy: >= 0 })
        {
            _lineH.Position = new Vector2f(_shift, Selected.iy * _step + _shift);
            _lineV.Position = new Vector2f(Selected.ix * _step + _shift, _shift);
            _lineH.Position += _sprite.Position;
            _lineV.Position += _sprite.Position;
            target.Draw(_lineH);
            target.Draw(_lineV);
        }

        var size = Params.DeskSize;
        for (var ix = 0; ix < size; ++ix)
        for (var iy = 0; iy < size; ++iy)
        {
            //TODO: remove shift
            var shift = new Vector2f(2f, 2f);
            if (_desk[ix + 1, iy + 1] == Board.States.White)
            {
                _white.Position = new Vector2f(_sprite.Position.X - _step / 2f + _step * ix + _shift,
                    _sprite.Position.Y - _step / 2f + _step * iy + _shift);
                _white.Position += shift;
                target.Draw(_white);
            }
            else if (_desk[ix + 1, iy + 1] == Board.States.Black)
            {
                _black.Position = new Vector2f(_sprite.Position.X - _step / 2f + _step * ix + _shift,
                    _sprite.Position.Y - _step / 2f + _step * iy + _shift);
                _black.Position += shift;
                target.Draw(_black);
            }
        }
    }
}
