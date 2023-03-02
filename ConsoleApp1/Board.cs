using SFML.Graphics;
using SFML.System;

namespace ConsoleApp1;
class Board : Drawable
{
    private int _size;
    
    private Sprite? _sprite;

    public States[,] Desk;
    
    //TODO: use screen sizes
    public Board()
    {
        _size = Settings.DeskSize - 1;
        
        int step = (int)(900f / _size), size =  step * _size + 3;
        //TODO: add desk boarders
        var texture = new RenderTexture((uint)size, (uint)size);
        //TODO: set background picture
        texture.Clear(Color.Yellow); 
        
        RectangleShape lineH = new(), lineV = new();
        lineH.Size = new Vector2f((uint)size, 3);
        lineV.Size = new Vector2f(3, (uint)size);
        lineH.FillColor = Color.Black;
        lineV.FillColor = Color.Black;

        _size += 3;
        Desk = new States[_size, _size];
        for (var i = 0; i < _size; ++i)
        {
            lineH.Position = new Vector2f(0, i * step);
            lineV.Position = new Vector2f(i * step, 0);
            texture.Draw(lineH);
            texture.Draw(lineV);
            
            for (var j = 0; j < _size; ++j) 
                Desk[i, j] = States.Free;
            
            Desk[i, 0] = States.Edge;
            Desk[i, _size - 1] = States.Edge;
            Desk[0, i] = States.Edge;
            Desk[_size - 1, i] = States.Edge;
        }

        texture.Display();
        _sprite = new Sprite(texture.Texture);
        _sprite.Position = new Vector2f(
            );
    }

    public void MoveSelect(int x, int y)
    {
        
    }

    public void PutStone(int ix, int iy, bool isWhite)
    {
        if (isWhite) Desk[ix, iy] = States.White;
        else Desk[ix, iy] = States.Black;
        
        //TODO: check Ko rule
        //TODO: check if the stone surrounded
        //TODO: paint previous stone
    }
    
    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(_sprite);
    }
    
    public enum States : sbyte
    {
        White = 1,
        Black = -1,
        Free = 0,
        Edge = 2,
    }
}