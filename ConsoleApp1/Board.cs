﻿//using System.Diagnostics;
using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

public class Board
{
    public States[,] Desk;

    private States[,] _desk;

    public static (int ix, int iy) Selected;
    
    public Board(IArbiter? arbiter = null)
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
        _isWhite = false;
    }
    
    private IArbiter? _arbiter;

    private static bool _isWhite;

    public void PutStone()
    {
        if (Selected.ix < 0 || Selected.ix > Params.DeskSize ||
            Selected.iy < 0 || Selected.iy > Params.DeskSize)
            return;
        (var ix, var iy) = Selected;
        
        if (Desk[ix + 1, iy + 1] != States.Free) return;
        
        if (_isWhite) Desk[ix + 1, iy + 1] = States.White;
        else Desk[ix + 1, iy + 1] = States.Black;
        _isWhite = !_isWhite;
        
        //TODO: paint previous stone

        if (KoCheck(ix + 1, iy + 1)) return;
        
        //check 4 near stones
        if (_arbiter == null) return;
        _arbiter!.Capture(ref Desk,ix, iy + 1);
        _arbiter!.Capture(ref Desk,ix + 1, iy);
        _arbiter!.Capture(ref Desk,ix + 2, iy + 1); 
        _arbiter!.Capture(ref Desk,ix + 1, iy + 2);
        _arbiter!.Capture(ref Desk,ix + 1, iy + 1);
    }

    public void PutStone(int ix, int iy, bool isWhite)
    {
        if (_isWhite) Desk[ix + 1, iy + 1] = States.White;
        else Desk[ix + 1, iy + 1] = States.Black;
        
        if (_arbiter == null) return;
        _arbiter!.Capture(ref Desk,ix, iy + 1);
        _arbiter!.Capture(ref Desk,ix + 1, iy);
        _arbiter!.Capture(ref Desk,ix + 2, iy + 1); 
        _arbiter!.Capture(ref Desk,ix + 1, iy + 2);
        _arbiter!.Capture(ref Desk,ix + 1, iy + 1);
    }

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

        _isWhite = !_isWhite;
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
