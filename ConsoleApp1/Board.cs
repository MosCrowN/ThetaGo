﻿using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;

class Board
{
    public States[,] Desk;

    public static (int ix, int iy) Selected;
    
    public Board()
    {
        var size = Settings.DeskSize + 2;
        Desk = new States[size, size];
        for (var i = 0; i < size; ++i)
        {
            for (var j = 0; j < size; ++j)
                Desk[i, j] = States.Free;

            Desk[i, 0] = States.Edge;
            Desk[i, size - 1] = States.Edge;
            Desk[0, i] = States.Edge;
            Desk[size - 1, i] = States.Edge;
        }
    }

    private static bool _isWhite;
    
    public void PutStone(int ix = -1, int iy = -1)
    {
        if (ix == -1 || iy == -1) (ix, iy) = Selected;
        if (ix < 0 || iy < 0 || ix > Settings.DeskSize || iy > Settings.DeskSize)
            return;

        if (_isWhite) Desk[ix + 1, iy + 1] = States.White;
        else Desk[ix + 1, iy + 1] = States.Black;
        _isWhite = !_isWhite;

        //TODO: check Ko rule
        //TODO: check if the stone surrounded
        //TODO: paint previous stone
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
    private int _step, _shift;
    
    private Sprite _sprite, _black, _white;

    private RectangleShape _lineH, _lineV;

    private Board.States[,] _desk;
    
    public (int ix, int iy) Selected
    {
        set
        {
            var ix = (int)Math.Round((value.ix - _sprite.Position.X - _shift) / _step);
            var iy = (int)Math.Round((value.iy - _sprite.Position.Y - _shift) / _step);
            ix = ix >= Settings.DeskSize ? -1 : (ix < 0 ? -1 : ix);  
            iy = iy >= Settings.DeskSize ? -1 : (iy < 0 ? -1 : iy);
            Board.Selected = (ix, iy);
        }
        get => Board.Selected;
    }

    public BoardSprite(in Board board, int screenX, int screenY)
    {
        _desk = board.Desk;

        var min = Math.Min(screenX, screenY);
        var size = (int)(min * 5 / 6f);
        _shift = _step =  size / (Settings.DeskSize - 1);
        var texture = new RenderTexture((uint)(size + 3 + 2 * _shift),
            (uint)(size + 3 + 2 * _shift));
        texture.Clear();
        
        var bg = new Sprite(Settings.DeskBg);
        bg.Scale = new Vector2f((float)texture.Size.X / Settings.DeskBg.Size.X,
            (float)texture.Size.Y / Settings.DeskBg.Size.Y);
        texture.Draw(bg);

        _lineH = new RectangleShape();
        _lineV = new RectangleShape();
        _lineH.Size = new Vector2f((uint)(_step * (Settings.DeskSize - 1) + 3), 3);
        _lineV.Size = new Vector2f(3, (uint)(_step * (Settings.DeskSize - 1) + 3));
        _lineH.FillColor = Color.Black;
        _lineV.FillColor = Color.Black;

        for (var i = 0; i < Settings.DeskSize; ++i)
        {
            _lineH.Position = new Vector2f( _shift, i * _step + _shift);
            _lineV.Position = new Vector2f(i * _step + _shift, _shift);
            texture.Draw(_lineH);
            texture.Draw(_lineV);
        }

        texture.Display();
        _sprite = new Sprite(texture.Texture);
        _sprite.Position = new Vector2f((min - size) / 2f - _shift, (min - size) / 2f - _shift);
        
        _black = new Sprite(Settings.BlackStone);
        _black.Scale = new Vector2f((float)_step / Settings.BlackStone.Size.X,
            (float)_step / Settings.BlackStone.Size.Y);
        _white = new Sprite(Settings.WhiteStone);
        _white.Scale = new Vector2f((float)_step / Settings.WhiteStone.Size.X,
            (float)_step / Settings.WhiteStone.Size.Y);
        
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

        var size = Settings.DeskSize - 1;
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