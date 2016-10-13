using System.Collections.Generic;

namespace Jupiter.Mvvm
{
    /// <summary>
    /// Обертка над Dictionary, нужна т.к. биндинг к Dictionary по ключу не работает с .NET Native
    /// </summary>
    public class OperationTokenCollection
    {
        private Dictionary<string, OperationToken> _tokens = new Dictionary<string, OperationToken>();

        public void Add(string key, OperationToken value)
        {
            _tokens.Add(key, value);
        }

        public bool IsRegistered(string key)
        {
            return _tokens.ContainsKey(key);
        }

        public OperationToken this[string key]
        {
            get { return _tokens[key]; }
        }
    }
}
