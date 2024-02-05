namespace Frame.Exec
{
    internal class Executor
    {
        private readonly List<IExecutable> _executors;
        private readonly List<IRun> _runsList;
        private readonly List<IInit> _initList;
        private readonly List<IOnExit> _onExitList;

        private Thread thread;

        public Executor()
        {
            _executors = new List<IExecutable>(96);
            _runsList = new List<IRun>(32);
            _initList = new List<IInit>(32);
            _onExitList = new List<IOnExit>(32);
        }
        public void Add(params IExecutable[] executors)
        {
            if (executors == null)
            {
                throw new ArgumentNullException(nameof(executors), "Cannot add null executors.");
            }

            foreach (var executor in executors)
            {
                if (executor == null)
                {
                    throw new ArgumentNullException(nameof(executor), "Cannot add a null executor.");
                }

                _executors.Add(executor);

                if (executor is IRun Run)
                {
                    _runsList.Add(Run);
                }

                if (executor is IInit Init)
                {
                    _initList.Add(Init);
                }

                if (executor is IOnExit OnExit)
                {
                    _onExitList.Add(OnExit);
                }
            }
        }

        public void Initialization()
        {
            foreach (var executor in _initList)
            {
                executor.Init();
            }
        }
        private void ThreadInit()
        {
            foreach (var executor in _initList)
            {
                executor.Init();
            }
        }

        public void Run()
        {
            foreach (var executor in _runsList)
            {
                executor.Run();
            }
        }
        public void OnExit()
        {
            foreach (var executor in _onExitList)
            {
                executor.OnExit();
            }
        }

        public virtual void Clear()
        {
            _executors.Clear();
            _runsList.Clear();
            _initList.Clear();
            _onExitList.Clear();
            thread.Join();
        }
    }
}