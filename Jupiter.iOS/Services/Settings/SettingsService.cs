using System;
using System.Runtime.CompilerServices;
using Foundation;
using Jupiter.Core.Services.Settings;
using Newtonsoft.Json;

namespace Jupiter.iOS
{
	public class SettingsService : ISettingsService
	{
		private static NSUserDefaults _prefs = NSUserDefaults.StandardUserDefaults;

		public static ISettingsService Local => new SettingsService();

		public T Get<T>([CallerMemberName] string key = null, T defaultValue = default(T))
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key), "Key must not be null");

			try
			{
				var value = _prefs.StringForKey(key);
				if (!string.IsNullOrEmpty(value))
				{
					return JsonConvert.DeserializeObject<T>(value);
				}

				return defaultValue;
			}
			catch
			{
				return defaultValue;
			}
		}

		public void Set<T>(T value, [CallerMemberName] string key = null)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key), "Key must not be null");

			string json = JsonConvert.SerializeObject(value);
			_prefs.SetString(json, key);
			_prefs.Synchronize();
		}
	}
}