using Microsoft.Extensions.Options;
using System;
using System.Configuration;
using System.Data;
using VirtualMind.NetTest.Arquitetura.Data.DbClient;
using VirtualMind.NetTest.BO.Settings;

namespace VirtualMind.NetTest.BO
{
    public static class ConnectionFactory
    {
        public static IDbConnection GetDbConnectionDefault(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentException("String de conexão 'conn_string_default' não encontrada.");
            }
            DbFactory db = new DbFactory(VirtualMind.NetTest.Arquitetura.Data.DbClient.DbType.MSSQL, connectionString);
            return db.DbConnection;
        }
    }
}
