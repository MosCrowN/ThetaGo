using SFML.Window;

namespace ConsoleApp1;

internal class SettingsWindow : Window
{
    public SettingsWindow()
    {
        Layout = new ButtonLayout();
        Layout.Add("MUSIC VOLUME: 100", 100);
        Layout.Add("DESK SIZE: 19", 35, 19);
        Layout.Add("DIFFICULTY: 10", 10);
        Layout.Add("MULTIPLAYER");
        Layout.Add("SAVE & BACK");
        Layout.Compile(0, (int)SfWindow.Size.X/ 2, 0, (int)SfWindow.Size.Y);
    }
    
    public override void Loop()
    {
        Params.Ost.Play();

        while (IsOpen)
        {
            SfWindow.DispatchEvents();

            SfWindow.Clear();
            SfWindow.Draw(Layout);
            SfWindow.Display();
        }
        
        Params.Ost.Pause();
    }

    protected override void SfWindowOnMouseMoved(object? sender, MouseMoveEventArgs e)
    {
        if (Layout == null) return;
        Select = Layout.Selected(e.X, e.Y, IsMousePressed);
        if (!IsMousePressed) return;
        switch (Select)
        {
            case 0:
                Params.MusicVolume = Layout!.Slider;
                Params.Ost.Volume = Params.MusicVolume;
                break;
            case 1:
                Params.DeskSize = Layout!.Slider; 
                break;
        }
    }

    protected override void SfWindowOnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {
        if (e.Button == Mouse.Button.Left) IsMousePressed = true;
        if (Layout == null) return;
        Select = Layout.Selected(e.X, e.Y, true);
        switch (Select)
        {
            case 0:
                Params.MusicVolume = Layout!.Slider;
                Params.Ost.Volume = Params.MusicVolume;
                break;
            case 1:
                Params.DeskSize = Layout!.Slider;
                break;
        }
    }

    protected override void SfWindowOnMouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {
        IsMousePressed = false;
        if (e.Button != Mouse.Button.Left) return;
        switch (Select)
        {
            case 4:
                IsOpen = false;
                break;
        }
    }

    protected override void SfWindowOnKeyReleased(object? sender, KeyEventArgs e)
    {
        switch (e.Code)
        {
            case Keyboard.Key.Escape:
                IsOpen = false;
                break;
        }
    }
}
