using System;
using System.Collections.Generic;
using System.Data;
using VirtualMind.NetTest.Arquitetura.Library.Interface;
using VirtualMind.NetTest.Arquitetura.Library.Util;
using VirtualMind.NetTest.Arquitetura.Library.Util.Security;

namespace VirtualMind.NetTest.Arquitetura.Library
{
   
    public class NativeDAO<T> : INativeDAO<T>
    {

        private ObjectSecurity _ObjectSecurity;


        public ObjectSecurity ObjectSecurity
        {
            get { return _ObjectSecurity; }
        }


        private IDbConnection _connection;

        protected IDbTransaction _transaction;

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        public NativeDAO(IDbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            _connection = connection;
            _transaction = null;
        }

        ~NativeDAO()
        {
            Dispose();
        }

        public T ToObject(IDataReader dr)
        {
            T obj = (T)Instance.Create(typeof(T), null);
            for (int n = 0; n < dr.FieldCount; n++)
            {
                if (dr[n] != System.DBNull.Value)
                {
                    if (obj.GetType().Name.ToLower() != dr.GetName(n).ToLower())
                        Instance.Set(obj, ToClassPropertyName(dr.GetName(n)), dr[n]);
                    else
                        Instance.Set(obj, ToClassPropertyName(dr.GetName(n)) + "name", dr[n]);
                }
            }

            return obj;
        }

        public T ToObject(IDataReader dr, out int pNumRegTotal)
        {
            pNumRegTotal = 0;
            T obj = (T)Instance.Create(typeof(T), null);
            for (int n = 0; n < dr.FieldCount; n++)
            {
                if (dr.GetName(n) == "TotalRegistro")
                    pNumRegTotal = (int)dr[n];
                else if (dr[n] != System.DBNull.Value)
                {
                    if (obj.GetType().Name.ToLower() != dr.GetName(n).ToLower())
                        Instance.Set(obj, ToClassPropertyName(dr.GetName(n)), dr[n]);
                    else
                        Instance.Set(obj, ToClassPropertyName(dr.GetName(n)) + "name", dr[n]);
                }
            }

            return obj;
        }

        public U ToObject<U>(IDataReader dr)
        {
            U obj = (U)Instance.Create(typeof(U), null);
            for (int n = 0; n < dr.FieldCount; n++)
                if (dr[n] != System.DBNull.Value)
                {
                    if (obj.GetType().Name.ToLower() != dr.GetName(n).ToLower())
                        Instance.Set(obj, ToClassPropertyName(dr.GetName(n)), dr[n]);
                    else
                        Instance.Set(obj, ToClassPropertyName(dr.GetName(n)) + "name", dr[n]);
                }
            return obj;
        }

        protected string ToClassPropertyName(string nomeObjetoDB)
        {
            return nomeObjetoDB;
        }

        protected string ToObjectDatabaseName(string nomeClasseProp)
        {
            int n = 1;
            while (n < nomeClasseProp.Length)
            {
                if (nomeClasseProp.Substring(n, 1) == nomeClasseProp.Substring(n, 1).ToUpper())
                {
                    nomeClasseProp = nomeClasseProp.Insert(n, "_");
                    n++;
                }

                n++;
            }
            return nomeClasseProp.ToUpper();
        }

        #region IDisposable Members


        public void Dispose()
        {
            
        }

        #endregion

        #region INativeDAO<T,ID> Members

        public virtual void RollbackTransaction()
        {
            lock (typeof(INativeDAO<T>))
            {
                if (GetHashCodeControler() == this.GetHashCode())
                {
                    _transaction = GetTransactionControler();
                    _transaction.Rollback();
                    _transaction = null;
                    FreeHashCodeAndTransctionControler();
                    CloseConnection();
                }
            }
        }
        public virtual void BeginTransaction()
        {
            lock (typeof(INativeDAO<T>))
            {
                if (GetHashCodeControler() == null)
                {
                    checkIfConnectionIsOpen();
                    _transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    SetHashCodeControler();
                    SetTransactionControler();
                }
            }
        }

        public virtual void CommitTransaction()
        {
            lock (typeof(INativeDAO<T>))
            {
                if (GetHashCodeControler() == this.GetHashCode())
                {
                    _transaction = GetTransactionControler();
                    _transaction.Commit();
                    _transaction = null;
                    FreeHashCodeAndTransctionControler();

                    CloseConnection();
                }
            }
        }

        /// <summary>
        /// Checks if connection is open.
        /// </summary>
        protected void checkIfConnectionIsOpen()
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
        }
        protected void CloseConnection()
        {
            if (checkCanCloseConnection())
                Connection.Close();
        }

        private bool checkCanCloseConnection()
        {
            if (Connection.State != ConnectionState.Open)
                return false;

            if (QtdTransacao() > 0)
                return false;

            return true;
        }
        private int QtdTransacao()
        {
            
            return GetTransactionControler() == null ? 0 : 1;

            //IDbCommand comm = Connection.CreateCommand();

            //try
            //{
            //    comm.Transaction = GetTransactionControler();
            //    comm.CommandType = CommandType.Text;
            //    comm.CommandText = string.Format("SELECT @@TRANCOUNT AS Qtd_Tran");
            //    return (int)comm.ExecuteScalar();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    comm.Dispose();
            //}
        }

        /// <summary>
        /// Sets the hash code controler.
        /// </summary>
        private void SetHashCodeControler()
        {
            CallContext.SetData("HashCode", this.GetHashCode());
        }
        /// <summary>
        /// Sets the transaction.
        /// </summary>
        private void SetTransactionControler()
        {
            CallContext.SetData("Transaction", _transaction);
        }

        /// <summary>
        /// Gets the hash code controler.
        /// </summary>
        /// <returns></returns>
        private int? GetHashCodeControler()
        {
            return (int?)CallContext.GetData("HashCode");
        }
        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <returns></returns>
        private IDbTransaction GetTransactionControler()
        {
            return (IDbTransaction)CallContext.GetData("Transaction");
        }
        /// <summary>
        /// Frees the hash code controler.
        /// </summary>
        private void FreeHashCodeAndTransctionControler()
        {
            CallContext.FreeNamedDataSlot("HashCode");
            CallContext.FreeNamedDataSlot("Transaction");
        }

        #region "Persist methods Stored procedure"

        public static DbType TypeToDbType(Type t)
        {
            DbType dbt;
            try
            {
                if (t.Name.ToUpper() == "BYTE[]")
                    dbt = DbType.Binary;
                else
                    dbt = (DbType)Enum.Parse(typeof(DbType), t.Name);
            }
            catch
            {
                dbt = DbType.Object;
            }
            return dbt;
        }

        public T ExecuteReturnT(StatementDAO pStatement)
        {
            IDbCommand comm = Connection.CreateCommand();
            switch (pStatement.TypeCommand)
            {
                case TypeCommand.Text:
                    comm.CommandType = CommandType.Text;
                    break;
                case TypeCommand.TableDirect:
                    comm.CommandType = CommandType.TableDirect;
                    break;
                default:
                    comm.CommandType = CommandType.StoredProcedure;
                    break;
            }
            comm.CommandText = string.Format(pStatement.SQL);

            for (int n = 0; n < pStatement.NamesParameter.Count; n++)
            {
                IDbDataParameter param = comm.CreateParameter();
                param.ParameterName = "@" + pStatement.NamesParameter[n];
                param.DbType = TypeToDbType(pStatement.TypesParameter[n]);
                if (pStatement.TypesParameter[n] == DateTime.Now.GetType())  // Se o tipo de dado for Datetime
                {
                    if (pStatement.ValuesParameter[n] == null) //Se a data for null informa null
                        param.Value = DBNull.Value;
                    else if ((DateTime)pStatement.ValuesParameter[n] == DateTime.MinValue) //se a data for MinValue
                        param.Value = DBNull.Value;
                    else
                        param.Value = pStatement.ValuesParameter[n];
                }
                else //se for outro tipo de dado diferente de datetime
                {
                    param.Value = pStatement.ValuesParameter[n] == null ? DBNull.Value : pStatement.ValuesParameter[n];
                }

                comm.Parameters.Add(param);
            }

            comm.Transaction = GetTransactionControler();
            checkIfConnectionIsOpen();
            IDataReader dr = comm.ExecuteReader();
            T obj = default(T);
            try
            {

                while (dr.Read())
                {
                    obj = ToObject(dr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //return default(T);
            }
            finally
            {
                dr.Close();
                dr.Dispose();
                comm.Dispose();
                CloseConnection();
            }
            return obj;
        }

 
        public IList<T> ExecuteReturnListT(StatementDAO pStatement)
        {
            IDbCommand comm = Connection.CreateCommand();
            switch (pStatement.TypeCommand)
            {
                case TypeCommand.Text:
                    comm.CommandType = CommandType.Text;
                    break;
                case TypeCommand.TableDirect:
                    comm.CommandType = CommandType.TableDirect;
                    break;
                default:
                    comm.CommandType = CommandType.StoredProcedure;
                    break;
            }
            comm.CommandText = string.Format(pStatement.SQL);

            for (int n = 0; n < pStatement.NamesParameter.Count; n++)
            {
                IDbDataParameter param = comm.CreateParameter();
                param.ParameterName = "@" + pStatement.NamesParameter[n];
                param.DbType = TypeToDbType(pStatement.TypesParameter[n]);
                if (pStatement.TypesParameter[n] == DateTime.Now.GetType())  // Se o tipo de dado for Datetime
                {
                    if (pStatement.ValuesParameter[n] == null) //Se a data for null informa null
                        param.Value = DBNull.Value;
                    else if ((DateTime)pStatement.ValuesParameter[n] == DateTime.MinValue) //se a data for MinValue
                        param.Value = DBNull.Value;
                    else
                        param.Value = pStatement.ValuesParameter[n];
                }
                else 
                {
                    param.Value = pStatement.ValuesParameter[n] == null ? DBNull.Value : pStatement.ValuesParameter[n];
                }
                comm.Parameters.Add(param);
            }

            comm.Transaction = GetTransactionControler();

            //comm.CommandText = string.Format(pStatement.SQL);
            checkIfConnectionIsOpen();
            IDataReader dr = comm.ExecuteReader();

            List<T> lst = new List<T>();
            try
            {
                while (dr.Read())
                {
                    T obj = ToObject(dr);
                    lst.Add(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //return new List<T>();
            }
            finally
            {
                comm.Dispose();
                dr.Close();
                dr.Dispose();
                CloseConnection();
            }
            return lst;
        }


        public IList<T> ExecuteReturnListT(StatementDAO pStatement, out int pNumRegTotal)
        {
            pNumRegTotal = 0;
            IDbCommand comm = Connection.CreateCommand();
            switch (pStatement.TypeCommand)
            {
                case TypeCommand.Text:
                    comm.CommandType = CommandType.Text;
                    break;
                case TypeCommand.TableDirect:
                    comm.CommandType = CommandType.TableDirect;
                    break;
                default:
                    comm.CommandType = CommandType.StoredProcedure;
                    break;
            }
            comm.CommandText = string.Format(pStatement.SQL);

            for (int n = 0; n < pStatement.NamesParameter.Count; n++)
            {
                IDbDataParameter param = comm.CreateParameter();
                param.ParameterName = "@" + pStatement.NamesParameter[n];
                param.DbType = TypeToDbType(pStatement.TypesParameter[n]);
                if (pStatement.TypesParameter[n] == DateTime.Now.GetType())  // Se o tipo de dado for Datetime
                {
                    if (pStatement.ValuesParameter[n] == null) //Se a data for null informa null
                        param.Value = DBNull.Value;
                    else if ((DateTime)pStatement.ValuesParameter[n] == DateTime.MinValue) //se a data for MinValue
                        param.Value = DBNull.Value;
                    else
                        param.Value = pStatement.ValuesParameter[n];
                }
                else //se for outro tipo de dado diferente de datetime
                {
                    param.Value = pStatement.ValuesParameter[n] == null ? DBNull.Value : pStatement.ValuesParameter[n];
                }
                comm.Parameters.Add(param);
            }

            comm.Transaction = GetTransactionControler();

            //comm.CommandText = string.Format(pStatement.SQL);
            checkIfConnectionIsOpen();
            IDataReader dr = comm.ExecuteReader();

            List<T> lst = new List<T>();
            try
            {
                while (dr.Read())
                {
                    T obj = ToObject(dr, out pNumRegTotal);
                    lst.Add(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //return new List<T>();
            }
            finally
            {
                comm.Dispose();
                dr.Close();
                dr.Dispose();
                CloseConnection();
            }
            return lst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pStatement"></param>
        /// <returns></returns>
        public Object ExecuteScalar(StatementDAO pStatement)
        {

            IDbCommand comm = Connection.CreateCommand();
            switch (pStatement.TypeCommand)
            {
                case TypeCommand.Text:
                    comm.CommandType = CommandType.Text;
                    break;
                case TypeCommand.TableDirect:
                    comm.CommandType = CommandType.TableDirect;
                    break;
                default:
                    comm.CommandType = CommandType.StoredProcedure;
                    break;
            }
            comm.CommandText = string.Format(pStatement.SQL);

            for (int n = 0; n < pStatement.NamesParameter.Count; n++)
            {
                IDbDataParameter param = comm.CreateParameter();
                param.ParameterName = "@" + pStatement.NamesParameter[n];
                param.DbType = TypeToDbType(pStatement.TypesParameter[n]);
                if (pStatement.TypesParameter[n] == DateTime.Now.GetType())  // Se o tipo de dado for Datetime
                {
                    if (pStatement.ValuesParameter[n] == null) //Se a data for null informa null
                        param.Value = DBNull.Value;
                    else if ((DateTime)pStatement.ValuesParameter[n] == DateTime.MinValue) //se a data for MinValue
                        param.Value = DBNull.Value;
                    else
                        param.Value = pStatement.ValuesParameter[n];
                }
                else //se for outro tipo de dado diferente de datetime
                {
                    param.Value = pStatement.ValuesParameter[n] == null ? DBNull.Value : pStatement.ValuesParameter[n];
                }
                comm.Parameters.Add(param);
            }
            try
            {
                comm.Transaction = GetTransactionControler();
                //comm.CommandText = string.Format(pStatement.SQL);
                checkIfConnectionIsOpen();
                return comm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                comm.Dispose();
                CloseConnection();
            }
        }

        public int ExecuteNonQuery(StatementDAO pStatement)
        {
            IDbCommand comm = Connection.CreateCommand();
            switch (pStatement.TypeCommand)
            {
                case TypeCommand.Text:
                    comm.CommandType = CommandType.Text;
                    break;
                case TypeCommand.TableDirect:
                    comm.CommandType = CommandType.TableDirect;
                    break;
                default:
                    comm.CommandType = CommandType.StoredProcedure;
                    break;
            }
            comm.CommandText = string.Format(pStatement.SQL);

            for (int n = 0; n < pStatement.NamesParameter.Count; n++)
            {
                IDbDataParameter param = comm.CreateParameter();
                param.ParameterName = "@" + pStatement.NamesParameter[n];
                param.DbType = TypeToDbType(pStatement.TypesParameter[n]);
                if (pStatement.TypesParameter[n] == DateTime.Now.GetType())  // Se o tipo de dado for Datetime
                {
                    if (pStatement.ValuesParameter[n] == null) //Se a data for null informa null
                        param.Value = DBNull.Value;
                    else if ((DateTime)pStatement.ValuesParameter[n] == DateTime.MinValue) //se a data for MinValue
                        param.Value = DBNull.Value;
                    else
                        param.Value = pStatement.ValuesParameter[n];
                }
                else //se for outro tipo de dado diferente de datetime
                {
                    param.Value = pStatement.ValuesParameter[n] == null ? DBNull.Value : pStatement.ValuesParameter[n];
                }
                comm.Parameters.Add(param);
            }
            try
            {
                comm.Transaction = GetTransactionControler();
                comm.CommandText = string.Format(pStatement.SQL);
                checkIfConnectionIsOpen();
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                comm.Dispose();
                CloseConnection();
            }
        }

        #endregion
        #endregion
    }
}
