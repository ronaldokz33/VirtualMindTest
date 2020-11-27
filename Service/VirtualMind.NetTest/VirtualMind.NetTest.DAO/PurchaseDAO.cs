using System.Collections.Generic;
using System.Data;
using VirtualMind.NetTest.Arquitetura.Library;
using VirtualMind.NetTest.Arquitetura.Library.Util.Security;
using VirtualMind.NetTest.VO;

namespace VirtualMind.NetTest.DAO
{
    public class PurchaseDAO : NativeDAO<Purchase>
    {
        #region Constructor
        public PurchaseDAO(IDbConnection connection) : base(connection)
        {
        }

        #endregion

        #region Base Methods
        public Purchase InsertByStoredProcedure(Purchase pObject)
        {
            string sql = "[dbo].[I_sp_Purchase]";
            StatementDAO statement = new StatementDAO(sql);
            statement.AddParameter("userId", pObject.userId);
            statement.AddParameter("currency", pObject.currency);
            statement.AddParameter("amount", pObject.amount);

            return this.ExecuteReturnT(statement);
        }

        public IList<Purchase> ListForLookup(Purchase pObject)
        {
            string sql = "[dbo].[S_sp_Purchase_ListForLookup]";
            StatementDAO statement = new StatementDAO(sql);

            statement.AddParameter("userId", pObject.userId);
            statement.AddParameter("currency", pObject.currency);

            return this.ExecuteReturnListT(statement);
        }
        #endregion
    }
}
