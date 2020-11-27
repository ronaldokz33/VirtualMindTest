using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace VirtualMind.NetTest.Arquitetura.Data.DbClient
{
    public class ConnectionFactory : IDisposable
    {
        /// <summary>
        /// Classe protegida que armazena a conexão com o banco de dados.
        /// </summary>
        protected IDbConnection conn;

        /// <summary>
        /// Inicializa uma instância da classe <see cref="ConnectionFactory"/>.
        /// </summary>
        /// <param name="dbType">O tipo do banco de dados.</param>
        /// <param name="connectionString">A string de conexão.</param>
        public ConnectionFactory(DbType dbType, string connectionString)
        {
            if (dbType == DbType.MSSQL)
                conn = new SqlConnection(connectionString);
            else if (dbType == DbType.MYSQL)
                conn = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Lê a conexão com o banco de dados.
        /// </summary>
        /// <value>A conexão.</value>
        public IDbConnection DbConnection
        {
            get
            {
                return conn;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            conn.Dispose();
        }

        #endregion
    }
}
