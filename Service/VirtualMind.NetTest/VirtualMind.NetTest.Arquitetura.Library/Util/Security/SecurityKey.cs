using System;

namespace VirtualMind.NetTest.Arquitetura.Library.Util.Security
{
    public class SecurityKey
    {
        public SecurityKey(string pName, object pValue)
        {
            this.Name = pName;
            this.Value = pValue;
            this._ValueType = pValue.GetType();
        }

        public SecurityKey()
        {
        }

        private string _Name;
        private object _Value;
        private Type _ValueType;

        public Type ValueType
        {
            get { return _ValueType; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public object Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                this._ValueType = value.GetType();
            }
        }
    }
}
