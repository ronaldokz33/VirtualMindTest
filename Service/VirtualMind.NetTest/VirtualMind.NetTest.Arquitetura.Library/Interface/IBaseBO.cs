using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualMind.NetTest.Arquitetura.Library.Interface
{
    /// <summary>
    /// Define métodos de regras de negócios dos objetos.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="ID"></typeparam>
    public interface IBaseBO<T, ID> : IDisposable
    {
        #region "Metodos de Persistência Básicos"
        /// <summary>
        /// Realiza o insert do objeto
        /// </summary>
        /// <param name="pObject">Objeto a ser inserido</param>
        /// <returns>Objeto inserido</returns>
        T Insert(T pObject);

        /// <summary>
        /// Realiza o update do objeto
        /// </summary>
        /// <param name="pObject">Objeto a ser atualizado</param>
        /// <returns>Objeto Atualizado</returns>
        T Update(T pObject);

        /// <summary>
        /// Realiza a deleção do objeto
        /// </summary>
        /// <param name="pObject">Objeto contendo os valores dos campos que sao chave do objeto a ser deletado</param>
        /// <returns>Quantidade de Registros deletados</returns>
        int Delete(ID pChave);

        /// <summary>
        /// Realiza a busca pelos parametros informados no objeto por stored Procedure
        /// </summary>
        /// <param name="pObject">Objeto contendo os valores para o filtro</param>
        /// <param name="pNumRegPag">Número de registros por página</param>
        /// <param name="pNumPagina">Página corrente</param>
        /// <param name="pDesOrdem">Critério de ordenação</param>
        /// <param name="pNumTotReg">Quantidade de registros que a consulta retorna</param>
        /// <returns>Lista de Objetos que atendam ao filtro</returns>
        IList<T> ListForGrid(T pObject, int pNumRegPag, int pNumPagina, string pDesOrdem, out int pNumTotReg);

        /// <summary>
        /// Realiza a busca pelos parametros informados no objeto por stored Procedure
        /// </summary>
        /// <param name="pObject">Objeto contendo os valores para o filtro</param>
        /// <returns>Lista de Objetos que atendam ao filtro</returns>
        IList<T> ListForLookup(T pObject);

        /// <summary>
        /// Retorna registro pela PK
        /// </summary>
        /// <param name="pObject">PK da tabela</param>
        /// <returns>Registro da PK</returns>
        T SelectByPK(ID pChave);

        #endregion
    }
}
