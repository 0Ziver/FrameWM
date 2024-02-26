namespace Frame.Comands;

public interface ICommand
{
    void Invoke();
}

public interface ICommand<T>
{
    void Invoke(T arg);
}