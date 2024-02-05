namespace Frame.Exec
{
    public interface IExecutable { }
    public interface IRun : IExecutable {  void Run(); }
    public interface IInit : IExecutable { void Init(); }
    public interface IOnExit : IExecutable { void OnExit(); }
}