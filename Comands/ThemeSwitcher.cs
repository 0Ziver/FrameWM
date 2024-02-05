using Frame.Commands;
using static Frame.Helpers.LibImports;

namespace Frame.Comands;

public class ThemeSwitcher : ICommand
{
    private const int Light = 1;
    private const int Dark = 2;
    private bool toggle = false;

    public void Execute()
    {
        toggle = !toggle;
    }
}