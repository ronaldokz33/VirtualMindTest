using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace VirtualMind.NetTest.Arquitetura.Data.DbClient
{
    /// <summary>
    /// Classe que cria e gerencia dados de retorno em uma query do banco de dados.
    /// </summary>
    public class DataAdapterFactory
    {
        /// <summary>
        /// Classe protegida que armazenda os dados de retorno a partir de uma instrução SQL.
        /// </summary>
        protected IDbDataAdapter da;

        /// <summary>
        /// Inicializa uma instância da classe <see cref="DataAdapterFactory"/>.
        /// </summary>
        /// <param name="selectCommand">O comando SELECT.</param>
        public DataAdapterFactory(IDbCommand selectCommand)
        {
            if (selectCommand is SqlCommand)
                da = new SqlDataAdapter((selectCommand as SqlCommand));
            else if (selectCommand is MySqlCommand)
                da = new MySqlDataAdapter((selectCommand as MySqlCommand));
            else
                throw new ArgumentException("Banco de dados não suportado");
        }

        /// <summary>
        /// Lê o DataAdapter.
        /// </summary>
        /// <value>O DataAdapter.</value>
        public IDbDataAdapter DbDataAdapter
        {
            get
            {
                return da;
            }
        }
    }
}
