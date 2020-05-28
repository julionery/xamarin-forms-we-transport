using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace WeTransport.Helpers
{
    /// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string UserKey
        {
            get => AppSettings.GetValueOrDefault(nameof(UserKey), "");

            set => AppSettings.AddOrUpdateValue(nameof(UserKey), value);
        }

        public static string UserToken
        {
            get => AppSettings.GetValueOrDefault(nameof(UserToken), "");

            set => AppSettings.AddOrUpdateValue(nameof(UserToken), value);
        }

        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(nameof(UserName), "");

            set => AppSettings.AddOrUpdateValue(nameof(UserName), value);
        }

        public static string UserEmail
        {
            get => AppSettings.GetValueOrDefault(nameof(UserEmail), "");

            set => AppSettings.AddOrUpdateValue(nameof(UserEmail), value);
        }

        public static string UserType
        {
            get => AppSettings.GetValueOrDefault(nameof(UserType), "");

            set => AppSettings.AddOrUpdateValue(nameof(UserType), value);
        }

        public static bool isUser
        {
            get => AppSettings.GetValueOrDefault(nameof(isUser), false);

            set => AppSettings.AddOrUpdateValue(nameof(isUser), value);
        }

        public static bool isService
        {
            get => AppSettings.GetValueOrDefault(nameof(isService), false);

            set => AppSettings.AddOrUpdateValue(nameof(isService), value);
        }

        public static string SetQtdTotalItens(int total)
        {
            string QtdTotalItens = "";
            if (total > 0)
            {
                if (total > 1)
                    QtdTotalItens = string.Format("Total: {0} registros encontrados!", total.ToString());
                else
                    QtdTotalItens = string.Format("Total: {0} registro encontrado!", total.ToString());
            }
            else
                QtdTotalItens = "Nenhum registro encontrado!";

            return QtdTotalItens;
        }

    }
}
