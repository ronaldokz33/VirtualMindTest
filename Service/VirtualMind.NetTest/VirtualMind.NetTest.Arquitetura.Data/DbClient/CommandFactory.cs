using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace VirtualMind.NetTest.Arquitetura.Data.DbClient
{
    /// <summary>
    /// Classe que gerencia e executa comandos SQL enviados ao banco de dados.
    /// </summary>
    public class CommandFactory : IDisposable
    {
        /// <summary>
        /// Classe protegida que executa comandos SQL.
        /// </summary>
        protected IDbCommand comm;

        /// <summary>
        /// Inicializa uma instância da classe <see cref="CommandFactory"/>.
        /// </summary>
        /// <param name="conn">A conexão.</param>
        public CommandFactory(IDbConnection conn)
        {
            if (conn is SqlConnection)
                comm = new SqlCommand();
            else if (conn is MySqlConnection)
                comm = new MySqlCommand();
            comm.Connection = conn;
        }
        /// <summary>
        /// Inicializa uma instância da classe <see cref="CommandFactory"/>.
        /// </summary>
        /// <param name="conn">A conexão.</param>
        /// <param name="tran">A transação.</param>
        public CommandFactory(IDbConnection conn, IDbTransaction tran)
        {
            if (conn is SqlConnection && tran is SqlTransaction)
                comm = new SqlCommand();
            else if (conn is MySqlConnection && tran is MySqlTransaction)
                comm = new MySqlCommand();
            else
                throw new ArgumentException("Banco de dados não suportado");
            comm.Connection = conn;
            comm.Transaction = tran;
        }
        /// <summary>
        /// Inicializa uma instância da classe <see cref="CommandFactory"/>.
        /// </summary>
        /// <param name="conn">A conexão.</param>
        /// <param name="tran">A transação.</param>
        /// <param name="cmdText">O comando SQL.</param>
        public CommandFactory(IDbConnection conn, IDbTransaction tran, string cmdText)
        {
            if (conn is SqlConnection && tran is SqlTransaction)
                comm = new SqlCommand(cmdText, (conn as SqlConnection), (tran as SqlTransaction));
            else if (conn is MySqlConnection && tran is MySqlTransaction)
                comm = new MySqlCommand(cmdText, (conn as MySqlConnection), (tran as MySqlTransaction));
            else
                throw new ArgumentException("Banco de dados não suportado");
        }
        /// <summary>
        /// Lê o executor de comandos SQL.
        /// </summary>
        /// <value>O Command..</value>
        public IDbCommand DbCommand
        {
            get
            {
                return comm;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            comm.Dispose();
        }

        #endregion
    }
}
