using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace XFramework
{
    public sealed class TypesManager : Singleton<TypesManager>, IDisposable
    {
        /// <summary>
        /// ע����ĳ���
        /// </summary>
        private Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

        /// <summary>
        /// �洢���еı������ClassBaseAttribute����
        /// <para>Key : ��ǵ�Attribute</para>
        /// <para>Value : ����ǵ����Type�б�</para>
        /// </summary>
        private UnOrderMapSet<Type, Type> allTypes = new UnOrderMapSet<Type, Type>();

        /// <summary>
        /// ���м̳���IEvent(T)�ӿڵ���
        /// <para>Key : ���Type</para>
        /// <para>Value : T�����б�</para>
        /// </summary>
        private UnOrderMapSet<Type, Type> allEvents = new UnOrderMapSet<Type, Type>();

        /// <summary>
        /// ���м̳���UComponent(T)����
        /// <para>Key : ���Type.Name</para>
        /// <para>Value : Unity Component����</para>
        /// </summary>
        private Dictionary<string, Type> allUComponentNames = new Dictionary<string, Type>();

        /// <summary>
        /// ���м̳���UComponent(T)����
        /// <para>Key : ���Type</para>
        /// <para>Value : UComponent(T)����</para>
        /// </summary>
        private Dictionary<Type, Type> allUComponents = new Dictionary<Type, Type>();

        public void Init()
        {
            Add(this.GetType().Assembly);
        }

        public void Add(Assembly ass)
        {
            string assName = ass.FullName;
            assemblies[assName] = ass;

            allTypes.Clear();
            allEvents.Clear();
            foreach (Assembly assembly in assemblies.Values)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsAbstract)
                        continue;

                    //�ҵ����б����BaseAttribute���Ե��࣬������̳е�����
                    //Ȼ��浽allType����ȥ�����������ȡ������Ե�����
                    var clasAttris = type.GetCustomAttributes(typeof(BaseAttribute), true);
                    if (clasAttris.Length > 0)
                    {
                        foreach (BaseAttribute attri in clasAttris)
                        {
                            allTypes.Add(attri.AttributeType, type);
                        }
                    }

                    #region IEvent
                    var interfaces = type.GetInterfaces();
                    if (interfaces.Length > 0)
                    {
                        foreach (Type interfaceType in interfaces)
                        {
                            //Log.Info(interfaceType.Name);
                            //�˴��ǽ��̳���IEvent<T>�ӿڵ��������T�����ʹ��ڣ��������һ����ע���¼�
                            if (interfaceType.Name == "IEvent`1")
                            {
                                Type[] genericArguments = interfaceType.GetGenericArguments();
                                foreach (Type argumentType in genericArguments)
                                {
                                    allEvents.Add(type, argumentType);
                                }
                            }
                        }
                    }
                    #endregion
                }
            }

            SetUComponents();
        }

        /// <summary>
        /// ����UIComponent��Unity Component�Ĺ�����
        /// </summary>
        private void SetUComponents()
        {
            allUComponentNames.Clear();
            allUComponents.Clear();
            var types = GetTypes(typeof(UIComponentFlagAttribute));
            foreach (var type in types)
            {
                var t = type;

                // �ҵ�����UComponent(T)
                while (t.BaseType != null && t.BaseType.Name != "UComponent`1")
                {
                    t = t.BaseType;
                }

                var uCompType = t.BaseType;
                if (uCompType != null)
                {
                    // ��ȡ����T������
                    Type[] genericArguments = uCompType.GetGenericArguments();
                    Type unityComponentType = genericArguments[0];
                    allUComponentNames.Add(type.Name, unityComponentType);
                    allUComponents.Add(type, uCompType);
                }
            }
        }

        /// <summary>
        /// ��ȡ�����ָ�����Ե���
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public HashSet<Type> GetTypes(Type attributeType)
        {
            if (allTypes.TryGetValue(attributeType, out var set))
                return set;

            return new HashSet<Type>();
        }

        /// <summary>
        /// ��ȡ���м̳���IEvent(T)����T������
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public HashSet<Type> GetEvents(Type objectType)
        {
            if (allEvents.TryGetValue(objectType, out var set))
                return set;

            return new HashSet<Type>();
        }

        /// <summary>
        /// ���Ի�ȡ���м̳���IEvent(T)����T������
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryGetEvents(Type objectType, out HashSet<Type> result)
        {
            result = null;

            if (allEvents.TryGetValue(objectType, out var set))
                result = set;

            return result != null;
        }

        /// <summary>
        /// ����ͨ��UIComponent���ͻ�ȡ���������Unity Component
        /// </summary>
        /// <param name="uiCompType"></param>
        /// <param name="unityComponent"></param>
        /// <returns></returns>
        public bool TryGetUnityCompType(Type uiCompType, out Type unityComponent)
        {
            string name = uiCompType.Name;
            return allUComponentNames.TryGetValue(name, out unityComponent);
        }

        /// <summary>
        /// ����ͨ��UIComponent���ͻ�ȡ�������UIComponent(T)
        /// </summary>
        /// <param name="uiCompType"></param>
        /// <param name="uCompType"></param>
        /// <returns></returns>
        public bool TryGetUCompType(Type uiCompType, out Type uCompType)
        {
            return allUComponents.TryGetValue(uiCompType, out uCompType);
        }

        public void Dispose()
        {
            assemblies.Clear();
            allTypes.Clear();
            allUComponentNames.Clear();
            allUComponents.Clear();
            Instance = null;
        }
    }
}
