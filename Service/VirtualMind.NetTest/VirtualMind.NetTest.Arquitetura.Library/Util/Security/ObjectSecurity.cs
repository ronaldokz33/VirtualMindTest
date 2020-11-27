using System.Collections.Generic;

namespace VirtualMind.NetTest.Arquitetura.Library.Util.Security
{
    public class ObjectSecurity
    {
        private List<SecurityKey> _Keys;

        #region "Properts"

        public List<SecurityKey> Keys
        {
            get { return _Keys; }
            set { _Keys = value; }
        }

        public SecurityKey Key
        {
            set
            {
                this.AddKey(value);
            }
        }

        ///// <summary>
        ///// Tenant que o usuario esta usando
        ///// </summary>
        //public string IdtUserTenant
        //{
        //    get { return _IdtUserTenant; }
        //    set { _IdtUserTenant = value; }
        //}

        /// <summary>
        /// Codigo do usuario Logado
        /// </summary>
        public int UserSystem { get; set; }
        public int IdAluno { get; set; }
        /// <summary>
        /// Nome do usuario logado
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Login do Usuario Logado
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Senha do usuario logado
        /// </summary>
        public string MinhaParceria { get; set; }


        #endregion

        #region "methods"

        public void AddKey(SecurityKey pKey)
        {
            if (Keys == null)
            {
                Keys = new List<SecurityKey>();
            }

            //verifica se já existe uma chave com o mesmo nome
            SecurityKey oldKey = getKey(pKey.Name);
            if (oldKey != null)
                Keys.Remove(oldKey);

            Keys.Add(pKey);
        }

        public SecurityKey getKey(string pName)
        {
            if (Keys == null)
                return null;

            SecurityKey KeyFind = Keys.Find(
                delegate (SecurityKey obj)
                {
                    return obj.Name == pName;
                }
            );
            return KeyFind;
        }

        #endregion
    }
}
