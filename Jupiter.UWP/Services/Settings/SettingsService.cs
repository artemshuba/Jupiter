using System;
using System.Runtime.CompilerServices;
using Windows.Foundation.Collections;
using Windows.Storage;
using Jupiter.Core.Services.Settings;
using Newtonsoft.Json;

namespace Jupiter.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private static ISettingsService _local;
        private static ISettingsService _roaming;

        public static ISettingsService Local => _local ?? (_local = new SettingsService(ApplicationData.Current.LocalSettings.Values));

        public static ISettingsService Roaming => _roaming ?? (_roaming = new SettingsService(ApplicationData.Current.RoamingSettings.Values));

        protected IPropertySet Values { get; set; }

        private SettingsService(IPropertySet values)
        {
            Values = values;
        }

        public void Set<T>(T value, [CallerMemberName]string key = null)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key must not be null");

            var container = new ApplicationDataCompositeValue();
            var serializedValue = JsonConvert.SerializeObject(value);
            container["Value"] = serializedValue;
            Values[key] = container;
        }

        public T Get<T>([CallerMemberName] string key = null, T defaultValue = default(T))
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key must not be null");

            try
            {
                if (Values.ContainsKey(key))
                {
                    //если по указанному ключу контенер, достаем значение
                    var container = Values[key] as ApplicationDataCompositeValue;
                    if (container != null && container.ContainsKey("Value"))
                    {
                        var value = container["Value"] as string;
                        var converted = JsonConvert.DeserializeObject<T>(value);
                        return converted;
                    }
                    else
                    {
                        //иначе, например при обновлении старого приложения, там может сразу лежать значение, достаем его
                        if (Values[key].GetType() == typeof(T))
                            return (T)Values[key];
                    }
                }

                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}