using System;
using System.Data;

namespace VirtualMind.NetTest.Arquitetura.Library.Interface
{
    /// <summary>
    /// Define métodos para persistência de objetos utilizando SQL Nativo.
    /// </summary>
    /// <typeparam name="T">O tipo de objeto que a classe irá gerenciar</typeparam>
    /// <typeparam name="ID">O tipo do ID.</typeparam>
    public interface INativeDAO<T> : IDisposable
    {
        /// <summary>
        /// Retorna sessão do banco de dados.
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Cancela uma transação.
        /// </summary>
        void RollbackTransaction();
        /// <summary>
        /// Inicia uma transação com o banco de dados.
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Grava as informações da transação no banco.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Converte um DataReader em uma instância da classe.
        /// </summary>
        /// <typeparam name="U">O tipo qualquer.</typeparam>
        /// <param name="dr">O DataReader.</param>
        /// <returns>O objeto.</returns>
        U ToObject<U>(IDataReader dr);
        /// <summary>
        /// Converte um DataReader em uma instância da classe.
        /// </summary>
        /// <param name="dr">O DataReader.</param>
        /// <returns>O objeto da classe gerenciada por este objeto de acesso a dados (DAO).</returns>
        T ToObject(IDataReader dr);
    }
}
