namespace Frame.Commands
{
    internal interface ICommand<T>
    {
        void Execute(T arg);
    }
    internal interface ICommand
    {
        void Execute();
    }
}
