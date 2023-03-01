using SFML.Graphics;

namespace ConsoleApp1;
class Board : Drawable
{
    private int _size;
    private Sprite? _sprite;
    
    public sbyte[,] Desk;
    public Board(int size)
    {
        _size = size + 2;
        Desk = new sbyte[_size, _size];
        
        for (int i = 0; i < _size; ++i)
        {
            Desk[i, 0] = -2;
            Desk[i, _size - 1] = -2;
            Desk[0, i] = -2;
            Desk[_size - 1, i] = -2;
        }

        var texture = new RenderTexture(900, 900);
        texture.Clear();
        texture.Display();

        _sprite = new Sprite(texture.Texture);
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(_sprite);
    }
}