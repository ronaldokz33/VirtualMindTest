using System;
using System.Data;
using VirtualMind.NetTest.Arquitetura.Library.Util;
using VirtualMind.NetTest.Arquitetura.Library.Util.Security;

namespace VirtualMind.NetTest.Arquitetura.Data.DbClient
{   
    /// <summary>
    /// Classe para gerenciar conexão e envio de instruções SQL com o banco de dados.
    /// </summary>
    public class DbFactory : IDisposable
    {
        /// <summary>
        /// Classe protegida que armazena a conexão com o banco.
        /// </summary>
        protected IDbConnection conn;
        /// <summary>
        /// Classe protegida para envio de comandos ou instruções SQL para o banco de dados.
        /// </summary>
        protected IDbCommand cmd;

        /// <summary>
        /// Inicializa uma instância da classe <see cref="DbFactory"/>.
        /// </summary>
        /// <param name="dbType">O tipo do banco de dados.</param>
        /// <param name="connectionString">A string de conexão.</param>
        /// <param name="pObjectSecurity"> Usuario e senha para acesso ao banco de dados</param>
        public DbFactory(DbType dbType, string connectionString) //, ObjectSecurity pObjectSecurity)
        {
            var stringComCriptografia = Config.Get("CONNECTION_CRYPTO");

            if (!string.IsNullOrEmpty(stringComCriptografia) && stringComCriptografia == "S")
            {
                var connectionStringAux = CryptographyTool.Decifrar(connectionString);
                connectionString = connectionStringAux;
            }

            //if (pObjectSecurity != null)
            //{
            //    if (pObjectSecurity.getKey("DatabaseServerName") != null)
            //        connectionString = connectionString.Replace("@DatabaseServerName", pObjectSecurity.getKey("DatabaseServerName").Value.ToString());

            //    if (pObjectSecurity.getKey("DatabaseName") != null)
            //        connectionString = connectionString.Replace("@DatabaseName", pObjectSecurity.getKey("DatabaseName").Value.ToString());

            //    if (pObjectSecurity.getKey("DatabaseUserName") != null)
            //        connectionString = connectionString.Replace("@DatabaseUserName", pObjectSecurity.getKey("DatabaseUserName").Value.ToString());

            //    if (pObjectSecurity.getKey("DatabasePassword") != null)
            //    {
            //        string password = pObjectSecurity.getKey("DatabasePassword").Value.ToString();
            //        if (password.Length > 20)
            //        {
            //            CryptographyTool crypto = new CryptographyTool();
            //            password = crypto.DecryptString(password);
            //        }
            //        connectionString = connectionString.Replace("@DatabasePassword", password);
            //    }
            //}

            if (connectionString.Contains("@DatabaseServerName"))
                connectionString = connectionString.Replace("@DatabaseServerName", Config.Get("DatabaseServerName"));

            if (connectionString.Contains("@DatabaseName"))
                connectionString = connectionString.Replace("@DatabaseName", Config.Get("DatabaseName"));

            if (connectionString.Contains("@DatabaseUserName"))
                connectionString = connectionString.Replace("@DatabaseUserName", Config.Get("DatabaseUserName"));

            if (connectionString.Contains("@DatabasePassword"))
            {
                string password = Config.Get("DatabasePassword");
                if (password.Length > 20)
                {
                    password = CryptographyTool.Decifrar(password);
                }
                connectionString = connectionString.Replace("@DatabasePassword", password);
            }

            conn = new ConnectionFactory(dbType, connectionString).DbConnection;
            cmd = new CommandFactory(conn).DbCommand;
        }

        ///// <summary>
        ///// Inicializa uma instância da classe <see cref="DbFactory"/>.
        ///// </summary>
        ///// <param name="dbType">O tipo do banco.</param>
        ///// <param name="servidor">O servidor.</param>
        ///// <param name="datasource">O DataSource</param>
        ///// <param name="usuario">O login do usuário.</param>
        ///// <param name="senha">A senha.</param>
        //public DbFactory(DbType dbType, string servidor, string datasource, string usuario, string senha)
        //{
        //    string connectionString = string.Empty;

        //    if (dbType == DbType.MSSQL)
        //        connectionString = "Server={0};Initial Catalog={1};user id={2};password={3}";
        //    connectionString = string.Format(connectionString, new string[] { servidor, datasource, usuario, senha });

        //    conn = new ConnectionFactory(dbType, connectionString).DbConnection;
        //    cmd = new CommandFactory(conn).DbCommand;
        //}

        /// <summary>
        /// Testa a conexão.
        /// </summary>
        /// <returns>Se True, a conexão do banco está corretamente configurada.</returns>
        public bool TestConnection()
        {
            try
            {
                conn.Open();
            }
            catch
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
            return true;
        }

        /// <summary>
        /// Lê a conexão.
        /// </summary>
        /// <value>A conexão.</value>
        public IDbConnection DbConnection
        {
            get
            {
                return conn;
            }
        }

        /// <summary>
        /// Inicia uma transação.
        /// </summary>
        /// <param name="il">O nível de isolamento.</param>
        /// <returns>O objeto de transação.</returns>
        public IDbTransaction BeginTansaction(IsolationLevel il)
        {
            return conn.BeginTransaction(il);
        }

        /// <summary>
        /// Lê o objeto de comandos SQL.
        /// </summary>
        /// <value>O objeto de comandos.</value>
        public IDbCommand Command
        {
            get { return cmd; }
        }

        /// <summary>
        /// Lê um DataAdapter.
        /// </summary>
        /// <param name="comm">A conexão.</param>
        /// <returns>O DataAdapter.</returns>
        public IDbDataAdapter getDataAdapter(IDbCommand comm)
        {
            return new DataAdapterFactory(comm).DbDataAdapter;
        }
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="useTransaction">if set to <c>true</c> [use transaction].</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string sql, bool useTransaction)
        {
            DataSet dataSet = new DataSet("dataSet");
            this.conn.Open();
            IDbTransaction transaction = null;
            if (useTransaction)
            {
                transaction = this.conn.BeginTransaction();
            }
            try
            {
                IDbCommand dbCommand = new CommandFactory(this.conn).DbCommand;
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = sql;
                if (useTransaction)
                {
                    dbCommand.Transaction = transaction;
                }
                new DataAdapterFactory(dbCommand).DbDataAdapter.Fill(dataSet);
                if (useTransaction)
                {
                    transaction.Commit();
                }
            }
            catch
            {
                if (useTransaction)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                this.conn.Close();
            }
            if (dataSet.Tables.Count == 0)
            {
                throw new Exception("Nenhum DataTable gerado.");
            }
            return dataSet.Tables[0];
        }
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string sql)
        {
            return this.ExecuteQuery(sql, true);
        }
        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="useTransaction">if set to <c>true</c> [use transaction].</param>
        /// <returns></returns>
        public int ExecuteSQL(string sql, bool useTransaction)
        {
            int num = 0;
            this.conn.Open();
            IDbTransaction transaction = null;
            if (useTransaction)
            {
                transaction = this.conn.BeginTransaction();
            }
            try
            {
                IDbCommand dbCommand = new CommandFactory(this.conn).DbCommand;
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = sql;
                if (useTransaction)
                {
                    dbCommand.Transaction = transaction;
                }
                num = dbCommand.ExecuteNonQuery();
                if (useTransaction)
                {
                    transaction.Commit();
                }
            }
            catch
            {
                if (useTransaction)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                this.conn.Close();
            }
            return num;
        }
        /// <summary>
        /// Executa uma query.
        /// </summary>
        /// <param name="sql">A instrução SQL.</param>
        /// <returns>O DataTable com os dados retornados.</returns>
        public int ExecuteSQL(string sql)
        {
            return this.ExecuteSQL(sql, true);
        }
        /// <summary>
        /// Executes the statement.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="useTransaction">if set to <c>true</c> [use transaction].</param>
        public void ExecuteStatement(string sql, bool useTransaction)
        {
            DataSet dataSet = new DataSet("dataSet");
            this.conn.Open();
            IDbTransaction transaction = null;
            if (useTransaction)
            {
                transaction = this.conn.BeginTransaction();
            }
            try
            {
                IDbCommand dbCommand = new CommandFactory(this.conn).DbCommand;
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = sql;
                if (useTransaction)
                {
                    dbCommand.Transaction = transaction;
                }
                new DataAdapterFactory(dbCommand).DbDataAdapter.Fill(dataSet);
                if (useTransaction)
                {
                    transaction.Commit();
                }
            }
            catch
            {
                if (useTransaction)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                this.conn.Close();
            }
        }
        /// <summary>
        /// Executa uma instrução SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        public void ExecuteStatement(string sql)
        {
            this.ExecuteStatement(sql, true);
        }
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
        }

        #endregion
    }
}
