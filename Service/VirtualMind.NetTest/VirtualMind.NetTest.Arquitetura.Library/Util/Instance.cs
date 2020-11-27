using System;
using System.Reflection;

namespace VirtualMind.NetTest.Arquitetura.Library.Util
{
    /// <summary>
    /// Metódos estáticos utilizando 'Reflection' pra manipulção de instâncias.
    /// </summary>
    public static class Instance
    {
        /// <summary>
        /// Cria uma instância com tipo e parâmetros definidos.
        /// </summary>
        /// <param name="type">O tipo da instância a ser criada.</param>
        /// <param name="args">Os parâmetros da instância.</param>
        /// <returns>A instância. Uma conversão explicita será necessária.</returns>
        public static object Create(System.Type type, object[] args)
        {
            System.Type t = type.Assembly.GetType(type.FullName);
            return t.InvokeMember(string.Empty,
                BindingFlags.DeclaredOnly | BindingFlags.Public |
                BindingFlags.Instance | BindingFlags.CreateInstance, null, null, args);
        }
        /// <summary>
        /// Cria uma instância de uma classe presente em um assembly.
        /// </summary>
        /// <param name="assemblyFile">O assembly.</param>
        /// <param name="typeFullName">Nome completo do tipo.</param>
        /// <param name="args">Os argumentos.</param>
        /// <returns>A instância. Uma conversão explicita será necessária.</returns>
        public static object Create(string assemblyFile, string typeFullName, object[] args)
        {
            Assembly a = Assembly.LoadFrom(assemblyFile);
            System.Type type = a.GetType(typeFullName);
            return type.InvokeMember(string.Empty,
                BindingFlags.DeclaredOnly | BindingFlags.Public |
                BindingFlags.Instance | BindingFlags.CreateInstance, null, null, args);
        }
        /// <summary>
        /// Lê a o valor da propriedade em um objeto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <param name="propertyName">Nome da propriedade.</param>
        /// <returns></returns>
        public static object Get(object obj, string propertyName)
        {
            System.Type t = obj.GetType();
            PropertyInfo[] property = t.GetProperties();
            return t.InvokeMember(propertyName,
                BindingFlags.DeclaredOnly | BindingFlags.Public |
                BindingFlags.Instance | BindingFlags.GetProperty, null, obj, null);
        }
        /// <summary>
        /// Grava um valor na propriedade de um objeto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <param name="propertyName">Nome da propriedade.</param>
        /// <param name="value">O valor.</param>
        public static void Set(object obj, string propertyName, object value)
        {
            if (obj.GetType().GetProperty(propertyName) == null)
                throw new Exception(string.Format("Property {0} não encontrada no Objeto {1}", propertyName, obj.GetType()));

            if (obj.GetType().GetProperty(propertyName).PropertyType.IsEnum) // Se for Enumerator 
            {
                System.Type t = obj.GetType();
                PropertyInfo[] property = t.GetProperties();
                t.InvokeMember(propertyName,
                    BindingFlags.DeclaredOnly | BindingFlags.Public |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Enum.Parse(obj.GetType().GetProperty(propertyName).PropertyType, value.ToString()) });
            }
            else if (obj.GetType().GetProperty(propertyName).PropertyType.IsGenericType // Se for Enumerator Nullable
                && obj.GetType().GetProperty(propertyName).PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                && obj.GetType().GetProperty(propertyName).PropertyType.GetGenericArguments()[0].IsEnum)
            {
                System.Type t = obj.GetType();
                PropertyInfo[] property = t.GetProperties();
                t.InvokeMember(propertyName,
                    BindingFlags.DeclaredOnly | BindingFlags.Public |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Enum.Parse(obj.GetType().GetProperty(propertyName).PropertyType.GetGenericArguments()[0], value.ToString()) });
            }
            else
            {
                PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

                    object safeValue = (value == null) ? null : Convert.ChangeType(value, t);

                    propertyInfo.SetValue(obj, safeValue, null);
                }

                //propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                //obj.GetType().GetProperty(propertyName).SetValue(obj, value, null);

                // Codigo comentado
                //System.Type t = obj.GetType();
                //PropertyInfo[] property = t.GetProperties();
                //t.InvokeMember(propertyName,
                //    BindingFlags.DeclaredOnly | BindingFlags.Public |
                //    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { value });
            }
        }
        /// <summary>
        /// Executa um método de um objeto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <param name="methodName">Nome do método.</param>
        /// <param name="args">Os parâmetros do método.</param>
        /// <returns>O retorto do método. Uma conversão explicita pode ser necessária.</returns>
        public static object Method(object obj, string methodName, object[] args)
        {
            System.Type t = obj.GetType();
            PropertyInfo[] property = t.GetProperties();
            return t.InvokeMember(methodName,
                BindingFlags.DeclaredOnly | BindingFlags.Public |
                BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, args);
        }

    }
}
