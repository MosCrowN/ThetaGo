using SFML.Graphics;
namespace ConsoleApp1;

class Button: Drawable
{
    private Sprite _sprite;
    private int _x0, _y0, _x1, _y1;

    public Button(int x, int y, Texture texture)
    {
        _sprite = new Sprite(texture);
        _x0 = x;
        _y0 = y;
        _x1 = x + (int)texture.Size.X;
        _y1 = y + (int)texture.Size.Y;
    }

    public bool IsClicked(int x, int y) 
        => _x0 < x && _y0 < y && _x1 > x && _y1 > y;

    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(_sprite);
    }
}