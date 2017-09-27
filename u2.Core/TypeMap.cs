using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using u2.Core.Extensions;

namespace u2.Core
{
    public class TypeMap
    {
        public IList<FieldMap> Maps = new List<FieldMap>();
        public IList<ModelMap> ModelMaps = new List<ModelMap>();

        public Type EntityType { get; }
        public virtual string Alias { get; set; }

        public IDictionary<string, Type> CmsFields { get; } = new Dictionary<string, Type>();

        public Action<IContent, object> Action { get; protected set; }

        public IList<GroupAction> GroupActions { get; protected set; } = new List<GroupAction>();

        public TypeMap(Type type)
        {
            EntityType = type;
        }

        public virtual object Validate(object instance)
        {
            return instance == null ? Activator.CreateInstance(EntityType) : EntityType.IsInstanceOfType(instance) ? instance : null;
        }

        /// <summary>
        /// Map all public instance properties for the given object using naming convensions.
        /// </summary>
        /// <returns>This TypeMap object.</returns>
        public TypeMap All()
        {
            EntityType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanWrite && (x.PropertyType.IsValueType || x.PropertyType == typeof(string)))
                .Each(x =>
                {
                    AddMap(new FieldMap(x));
                });

            return this;
        }

        protected void AddMap(FieldMap map)
        {
            if (map == null)
                return;

            Maps.Add(map);
        }

    }

    public class TypeMap<T> : TypeMap
        where T : class, new()
    {
        private string _alias;

        public override object Validate (object instance)
        {
            return instance == null ? new T() : instance is T ? instance : null;
        }

        public TypeMap() : base(typeof(T)) { }

        public override string Alias => _alias ?? (_alias = typeof (T).Name.ToLowerInvariant());

        public TypeMap<T> AliasTo(string alias)
        {
            _alias = alias.ToLowerInvariant();
            return this;
        }

        /// <summary>
        /// Map a Umbraco property to a object property using Func. Use it to map properties from both ends that need post processing.
        /// </summary>
        /// <typeparam name="TP">Object property type (same as Umbraco property type.</typeparam>
        /// <param name="alias">Umbraco property alias.</param>
        /// <param name="property">Lambda expression for the object property, given the declaring object.</param>
        /// <param name="mapFunc">Func to convert a TI value to a TO value.</param>
        /// <param name="defaultVal">Default value if property is not present in the content.</param>
        /// <returns>This TypeMap object.</returns>
        public TypeMap<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP))
        {

            if (property != null)
            {
                var map = new FieldMap<T, TP>(alias, property)
                {
                    Convert = mapFunc,
                    Default = defaultVal
                };
                AddMap(map);
            }

            return this;
        }

        /// <summary>
        /// Map a CMS property to object properties using Run.
        /// </summary>
        /// <typeparam name="TP">CMS property type.</typeparam>
        /// <param name="alias">CMS property alias.</param>
        /// <param name="action">Run to do the mapping.</param>
        /// <returns></returns>
        public TypeMap<T> Act<TP>(Action<T, TP> action, string alias)
        {
            if (!string.IsNullOrWhiteSpace(alias))
            {
                Put<TP>(alias);

                GroupActions.Add(new GroupAction
                {
                    Aliases = new List<string> { alias },
                    Action = (x, y) => action((T)x, (TP)y[0])
                });
            }

            return this;
        }

        /// <summary>
        /// Map a CMS property to object properties using Run.
        /// </summary>
        /// <param name="action">Run to do the mapping.</param>
        /// <param name="alias1">CMS property alias for TP1.</param>
        /// <param name="alias2">CMS property alias for TP2.</param>
        /// <returns></returns>
        public TypeMap<T> Act<TP1, TP2>(Action<T, TP1, TP2> action, string alias1, string alias2)
        {
            if (!string.IsNullOrWhiteSpace(alias1) && !string.IsNullOrWhiteSpace(alias2))
            {
                Put<TP1>(alias1);
                Put<TP2>(alias2);

                GroupActions.Add(new GroupAction
                {
                    Aliases = new List<string> { alias1, alias2 },
                    Action = (x, y) => action((T)x, (TP1)y[0], (TP2)y[1])
                });
            }

            return this;
        }

        /// <summary>
        /// Map a CMS property to object properties using Run.
        /// </summary>
        /// <param name="action">Run to do the mapping.</param>
        /// <param name="alias1">CMS property alias for TP1.</param>
        /// <param name="alias2">CMS property alias for TP2.</param>
        /// <param name="alias3">CMS property alias for TP3.</param>
        /// <returns></returns>
        public TypeMap<T> Act<TP1, TP2, TP3>(Action<T, TP1, TP2, TP3> action, string alias1, string alias2, string alias3)
        {
            if (!string.IsNullOrWhiteSpace(alias1) && !string.IsNullOrWhiteSpace(alias2) && !string.IsNullOrWhiteSpace(alias3))
            {
                Put<TP1>(alias1);
                Put<TP2>(alias2);
                Put<TP3>(alias3);

                GroupActions.Add(new GroupAction
                {
                    Aliases = new List<string> { alias1, alias2, alias3 },
                    Action = (x, y) => action((T)x, (TP1)y[0], (TP2)y[1], (TP3)y[2])
                });
            }

            return this;
        }

        /// <summary>
        /// Map a CMS property to object properties using Run.
        /// </summary>
        /// <param name="action">Run to do the mapping.</param>
        /// <param name="alias1">CMS property alias for TP1.</param>
        /// <param name="alias2">CMS property alias for TP2.</param>
        /// <param name="alias3">CMS property alias for TP3.</param>
        /// <param name="alias4">CMS property alias for TP4.</param>
        /// <returns></returns>
        public TypeMap<T> Act<TP1, TP2, TP3, TP4>(Action<T, TP1, TP2, TP3, TP4> action, string alias1, string alias2, string alias3, string alias4)
        {
            if (!string.IsNullOrWhiteSpace(alias1) && !string.IsNullOrWhiteSpace(alias2) && !string.IsNullOrWhiteSpace(alias3) && !string.IsNullOrWhiteSpace(alias4))
            {
                Put<TP1>(alias1);
                Put<TP2>(alias2);
                Put<TP3>(alias3);
                Put<TP4>(alias4);

                GroupActions.Add(new GroupAction
                {
                    Aliases = new List<string> { alias1, alias2, alias3, alias4 },
                    Action = (x, y) => action((T)x, (TP1)y[0], (TP2)y[1], (TP3)y[2], (TP4)y[3])
                });
            }

            return this;
        }

        /// <summary>
        /// Map a CMS property to object properties using Run.
        /// </summary>
        /// <param name="action">Run to do the mapping.</param>
        /// <param name="alias1">CMS property alias for TP1.</param>
        /// <param name="alias2">CMS property alias for TP2.</param>
        /// <param name="alias3">CMS property alias for TP3.</param>
        /// <param name="alias4">CMS property alias for TP4.</param>
        /// <param name="alias5">CMS property alias for TP5.</param>
        /// <returns></returns>
        public TypeMap<T> Act<TP1, TP2, TP3, TP4, TP5>(Action<T, TP1, TP2, TP3, TP4, TP5> action, string alias1, string alias2, string alias3, string alias4, string alias5)
        {
            if (!string.IsNullOrWhiteSpace(alias1) 
                && !string.IsNullOrWhiteSpace(alias2) 
                && !string.IsNullOrWhiteSpace(alias3) 
                && !string.IsNullOrWhiteSpace(alias4)
                && !string.IsNullOrWhiteSpace(alias5))
            {
                Put<TP1>(alias1);
                Put<TP2>(alias2);
                Put<TP3>(alias3);
                Put<TP4>(alias4);
                Put<TP5>(alias5);

                GroupActions.Add(new GroupAction
                {
                    Aliases = new List<string> { alias1, alias2, alias3, alias4, alias5 },
                    Action = (x, y) => action((T)x, (TP1)y[0], (TP2)y[1], (TP3)y[2], (TP4)y[3], (TP5)y[4])
                });
            }

            return this;
        }

        /// <summary>
        /// Map a CMS property to object properties using Run.
        /// </summary>
        /// <param name="action">Run to do the mapping.</param>
        /// <param name="alias1">CMS property alias for TP1.</param>
        /// <param name="alias2">CMS property alias for TP2.</param>
        /// <param name="alias3">CMS property alias for TP3.</param>
        /// <param name="alias4">CMS property alias for TP4.</param>
        /// <param name="alias5">CMS property alias for TP5.</param>
        /// <param name="alias6">CMS property alias for TP6.</param>
        /// <returns></returns>
        public TypeMap<T> Act<TP1, TP2, TP3, TP4, TP5, TP6>(Action<T, TP1, TP2, TP3, TP4, TP5, TP6> action, string alias1, string alias2, string alias3, string alias4, string alias5, string alias6)
        {
            if (!string.IsNullOrWhiteSpace(alias1)
                && !string.IsNullOrWhiteSpace(alias2)
                && !string.IsNullOrWhiteSpace(alias3)
                && !string.IsNullOrWhiteSpace(alias4)
                && !string.IsNullOrWhiteSpace(alias5)
                && !string.IsNullOrWhiteSpace(alias6))
            {
                Put<TP1>(alias1);
                Put<TP2>(alias2);
                Put<TP3>(alias3);
                Put<TP4>(alias4);
                Put<TP5>(alias5);
                Put<TP6>(alias6);

                GroupActions.Add(new GroupAction
                {
                    Aliases = new List<string> { alias1, alias2, alias3, alias4, alias5, alias6 },
                    Action = (x, y) => action((T)x, (TP1)y[0], (TP2)y[1], (TP3)y[2], (TP4)y[3], (TP5)y[4], (TP6)y[5])
                });
            }

            return this;
        }

        public TypeMap<T> Act(Action<IContent, T> action)
        {
            Action = (x, y) => action(x, (T)y);
            return this;
        }
        
        public TypeMap<T> Copy<TP>()
            where TP : class, new()
        {
            AddMap(new FieldMapCopy
            {
                ContentType = typeof(TP)
            });
            return this;
        }

        /// <summary>
        /// Map all public instance properties for the given object using naming convensions.
        /// </summary>
        /// <returns>This TypeMap object.</returns>
        public new TypeMap<T> All()
        {
            base.All();
            return this;
        }

        /// <summary>
        /// To be used with All, to ignore a property doesn't need to be mapped. A map with Func can't be ignored.
        /// </summary>
        /// <typeparam name="TO">Object/Umbraco property type.</typeparam>
        /// <param name="property">Lambda expression for the object property, given the declaring object.</param>
        /// <returns></returns>
        public TypeMap<T> Ignore<TO>(Expression<Func<T, TO>> property)
        {
            if (property != null)
            {
                var info = property.ToInfo();

                if (info != null)
                {
                    var found = Maps.FirstOrDefault(x => x.Alias == info.Name.ToLowerInvariant());

                    if (found != null)
                        Maps.Remove(found);
                }
            }

            return this;
        }
        public TypeMap<T> Tie<TModel, TKey>(Action<T, IEnumerable<TModel>> actionModel, Func<TModel, TKey> funcKey, string alias = null)
            where TModel : class, new()
        {
            var modelMap = new ModelMap<T, TModel, TKey>(alias, actionModel, funcKey) as ModelMap;

            ModelMaps.Add(modelMap);

            return this;
        }

        public TypeMap<T> Tie<TModel>(Expression<Func<T, IEnumerable<TModel>>> expModel, string alias = null)
            where TModel : class, new()
        {
            var action = expModel.ToSetter();
            var modelMap = new ModelMap<T, TModel>(alias, action);
            ModelMaps.Add(modelMap);

            return this;
        }

        private void Put<TP>(string alias)
        {
            alias = alias.ToLowerInvariant();
            if (!CmsFields.ContainsKey(alias))
                CmsFields.Add(alias, typeof(TP));
        }
    }
}