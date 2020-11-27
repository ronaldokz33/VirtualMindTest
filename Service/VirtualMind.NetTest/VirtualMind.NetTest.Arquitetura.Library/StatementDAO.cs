using System;
using System.Collections.Generic;

namespace VirtualMind.NetTest.Arquitetura.Library
{

    public enum TypeCommand
    {
        StoredProcedure,
        Text,
        TableDirect
    }

    public class StatementDAO
    {
        #region "Fields"
        private string _sql;
        private List<string> _namesParameter;
        private List<object> _valuesParameter;
        private List<Type> _typesParameter;
        private TypeCommand _typeCommand;

        #endregion

        public StatementDAO(string pSql)
        {
            _namesParameter = new List<string>();
            _valuesParameter = new List<object>();
            _typesParameter = new List<Type>();
            _sql = pSql;
            _typeCommand = TypeCommand.StoredProcedure;
        }

        public StatementDAO(string pSql, TypeCommand pTypeCommand)
        {
            _namesParameter = new List<string>();
            _valuesParameter = new List<object>();
            _typesParameter = new List<Type>();
            _sql = pSql;
            _typeCommand = pTypeCommand;
        }

        #region "Properties"


        public string SQL
        {
            get { return _sql; }
            set { _sql = value; }
        }

        public List<string> NamesParameter
        {
            get { return _namesParameter; }
        }


        public List<object> ValuesParameter
        {
            get { return _valuesParameter; }
        }

        public List<Type> TypesParameter
        {
            get { return _typesParameter; }
        }

        public TypeCommand TypeCommand
        {
            get { return _typeCommand; }
            set { _typeCommand = value; }
        }
        #endregion

        #region "Methods"

        public void AddParameter(string pNameParameter, object pValuesParameter, Type pTypesParameter)
        {
            NamesParameter.Add(pNameParameter);
            ValuesParameter.Add(pValuesParameter);
            TypesParameter.Add(pTypesParameter);
        }
        public void AddParameter(string pNameParameter, string pValuesParameter)
        {
            Type pTypesParameter = string.Empty.GetType();
            NamesParameter.Add(pNameParameter);
            ValuesParameter.Add(pValuesParameter);
            TypesParameter.Add(pTypesParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        public void AddParameter(string pNameParameter, byte[] pValuesParameter)
        {
            byte[] exemplo = new byte[100];
            Type pTypesParameter = exemplo.GetType();

            AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        /// <param name="pPermitirValorZero"></param>
        public void AddParameter(string pNameParameter, Int16 pValuesParameter, bool pPermitirValorZero = false)
        {
            System.Int16 exemplo = 0;
            Type pTypesParameter = exemplo.GetType();

            if (pValuesParameter > 0 || pPermitirValorZero)
                AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
            else
                AddParameter(pNameParameter, null, pTypesParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        /// <param name="pPermitirValorZero"></param>
        public void AddParameter(string pNameParameter, Int32 pValuesParameter, bool pPermitirValorZero = false)
        {
            System.Int32 exemplo = 0;
            Type pTypesParameter = exemplo.GetType();

            if (pValuesParameter > 0 || pPermitirValorZero)
                AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
            else
                AddParameter(pNameParameter, null, pTypesParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        /// <param name="pPermitirValorZero"></param>
        public void AddParameter(string pNameParameter, Int64 pValuesParameter, bool pPermitirValorZero = false)
        {
            System.Int64 exemplo = 0;
            Type pTypesParameter = exemplo.GetType();

            if (pValuesParameter > 0 || pPermitirValorZero)
                AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
            else
                AddParameter(pNameParameter, null, pTypesParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        /// <param name="pPermitirValorZero"></param>
        public void AddParameter(string pNameParameter, Decimal pValuesParameter, bool pPermitirValorZero = false)
        {
            System.Decimal exemplo = 0;
            Type pTypesParameter = exemplo.GetType();

            if (pValuesParameter > 0 || pPermitirValorZero)
                AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
            else
                AddParameter(pNameParameter, null, pTypesParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        /// <param name="pPermitirValorZero"></param>
        public void AddParameter(string pNameParameter, Double pValuesParameter, bool pPermitirValorZero = false)
        {
            System.Double exemplo = 0;
            Type pTypesParameter = exemplo.GetType();

            if (pValuesParameter > 0 || pPermitirValorZero)
                AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
            else
                AddParameter(pNameParameter, null, pTypesParameter);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        public void AddParameter(string pNameParameter, Int16? pValuesParameter)
        {
            System.Int16 exemplo = 0;
            Type pTypesParameter = exemplo.GetType();

            AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        public void AddParameter(string pNameParameter, Int32? pValuesParameter)
        {
            System.Int32 exemplo = 0;
            Type pTypesParameter = exemplo.GetType();

            AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        public void AddParameter(string pNameParameter, Int64? pValuesParameter)
        {
            System.Int64 exemplo = 0;
            Type pTypesParameter = exemplo.GetType();

            AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        public void AddParameter(string pNameParameter, Decimal? pValuesParameter)
        {
            System.Decimal exemplo = 0;
            Type pTypesParameter = exemplo.GetType();

            AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        public void AddParameter(string pNameParameter, Double? pValuesParameter)
        {
            System.Double exemplo = 0;
            Type pTypesParameter = exemplo.GetType();

            AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        public void AddParameter(string pNameParameter, DateTime pValuesParameter)
        {
            System.DateTime exemplo = DateTime.Now;
            Type pTypesParameter = exemplo.GetType();

            AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
        }
        public void AddParameter(string pNameParameter, DateTime? pValuesParameter)
        {
            System.DateTime exemplo = DateTime.Now;
            Type pTypesParameter = exemplo.GetType();

            AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNameParameter"></param>
        /// <param name="pValuesParameter"></param>
        public void AddParameter(string pNameParameter, Boolean pValuesParameter)
        {
            System.Boolean exemplo = true;
            Type pTypesParameter = exemplo.GetType();

            AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
        }
        public void AddParameter(string pNameParameter, Boolean? pValuesParameter)
        {
            System.Boolean exemplo = true;
            Type pTypesParameter = exemplo.GetType();

            AddParameter(pNameParameter, pValuesParameter, pTypesParameter);
        }

        public void AddClauses(List<string> pNamesParameter, List<object> pValuesParameter, List<Type> pTypesParameter)
        {
            if ((pNamesParameter.Count == pValuesParameter.Count))
            {
                _namesParameter = pNamesParameter;
                _valuesParameter = pValuesParameter;
                _typesParameter = pTypesParameter;
            }
            else
            {
                throw new Exception("Quantidade de parametros dos List não são iguais.");
            }
        }
        #endregion

    }
}
