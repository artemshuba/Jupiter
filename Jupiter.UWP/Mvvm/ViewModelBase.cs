using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Jupiter.Services.Navigation;

namespace Jupiter.Mvvm
{
    public abstract class ViewModelBase : BindableBase, INavigable
    {
        private readonly OperationTokenCollection _opTokens = new OperationTokenCollection();

        public NavigationService NavigationService { get; set; }

        public OperationTokenCollection OpTokens
        {
            get { return _opTokens; }
        }

        protected ViewModelBase()
        {
            InitializeCommands();
        }

        public virtual void OnNavigatedTo(Dictionary<string, object> parameters, NavigationMode mode)
        {
        }

        public virtual void OnNavigatingFrom(NavigatingEventArgs e)
        {
        }

        /// <summary>
        /// A method for initialization commands
        /// </summary>
        protected virtual void InitializeCommands()
        {

        }

        #region OperationToken helpers

        protected void RegisterTasks(params string[] ids)
        {
            foreach (var id in ids)
            {
                _opTokens.Add(id, new OperationToken());
            }
        }

        protected OperationToken TaskStarted(string id)
        {
            if (!_opTokens.IsRegistered(id))
                _opTokens.Add(id, new OperationToken());

            _opTokens[id].Error = null;
            _opTokens[id].IsWorking = true;

            return _opTokens[id];
        }

        protected void TaskFinished(string id)
        {
            _opTokens[id].IsWorking = false;
        }

        protected void TaskError(string id, string error)
        {
            _opTokens[id].Error = error;
            _opTokens[id].IsWorking = false;
        }

        #endregion
    }
}