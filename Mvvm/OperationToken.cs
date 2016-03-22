namespace Jupiter.Mvvm
{
    public class OperationToken : BindableBase
    {
        private bool _isWorking;
        private string _error;

        public bool IsWorking
        {
            get { return _isWorking; }
            set { Set(ref _isWorking, value); }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                if (Set(ref _error, value))
                    IsWorking = false;
            }
        }

        public void Finish()
        {
            IsWorking = false;
        }
    }
}