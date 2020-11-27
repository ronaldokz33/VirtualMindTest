using System.Configuration;

namespace VirtualMind.NetTest.Arquitetura.Library.Util
{
    /// <summary>
    /// Classe com métodos estáticos para ler e gravar dados na sessão 'AppSettings' do App.config ou web.config.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Define o valor para a chave e grava no arquivo de configuração.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <param name="value">O valor.</param>
        public static void Set(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        /// <summary>
        /// Ler o conteúdo da chave no arquivo de configuração.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <returns>O valor no arquivo de configuração.</returns>
		public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// Ler o conteúdo da chave no arquivo de configuração.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <returns>O valor no arquivo de configuração.</returns>
        public static string Get(int key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
