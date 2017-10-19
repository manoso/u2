using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using u2.Core.Contract;
using u2.Core.Extensions;

namespace u2.Core
{
    public abstract class MapTask : BaseTask, IMapTask
    {
        public string Alias { get; protected set; }

        public Type EntityType { get; protected set; }

        public IList<IModelMap> ModelMaps { get; } = new List<IModelMap>();

        public IDictionary<string, Type> CmsFields { get; } = new Dictionary<string, Type>();

        public IList<IGroupAction> GroupActions { get; protected set; } = new List<IGroupAction>();

        public Action<IContent, object> Action { get; protected set; }

        protected MapTask(Type type)
        {
            EntityType = type;
            Alias = type.Name.ToLowerInvariant();
        }

        public abstract object Create(object instance = null);

        /// <summary>
        /// Map all public instance properties for the given object using naming convensions.
        /// </summary>
        /// <returns>This MapTask object.</returns>
        public IMapTask All()
        {
            EntityType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanWrite && (x.PropertyType.IsValueType || x.PropertyType == typeof(string)))
                .Each(x =>
                {
                    AddMap(new MapItem(x));
                });

            return this;
        }
    }

    public class MapTask<T> : MapTask, IMapTask<T> where T : class, new()
    {
        public override object Create (object instance = null)
        {
            return instance is T ? instance : new T();
        }

        public MapTask() : base(typeof(T)) { }

        public IMapTask<T> AliasTo(string alias)
        {
            if (!string.IsNullOrWhiteSpace(alias))
                Alias = alias.ToLowerInvariant();
            return this;
        }

        public IMapTask<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP))
        {
            var map = CreatItem(property, alias, mapFunc, defaultVal);
            if (map != null)
                AddMap(map);

            return this;
        }

        /// <summary>
        /// Map a CMS property to object properties using Run.
        /// </summary>
        /// <typeparam name="TP">CMS property type.</typeparam>
        /// <param name="alias">CMS property alias.</param>
        /// <param name="action">Run to do the mapping.</param>
        /// <returns></returns>
        public IMapTask<T> Act<TP>(Action<T, TP> action, string alias)
        {
            if (!string.IsNullOrWhiteSpace(alias))
            {
                Put<TP>(alias);

                GroupActions.Add(new GroupAction
                {
                    Aliases = new List<string> { alias },
                    Action = (x, y) => action((T)x, y[0].To<TP>())
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
        public IMapTask<T> Act<TP1, TP2>(Action<T, TP1, TP2> action, string alias1, string alias2)
        {
            if (!string.IsNullOrWhiteSpace(alias1) && !string.IsNullOrWhiteSpace(alias2))
            {
                Put<TP1>(alias1);
                Put<TP2>(alias2);

                GroupActions.Add(new GroupAction
                {
                    Aliases = new List<string> { alias1, alias2 },
                    Action = (x, y) => action((T)x, y[0].To<TP1>(), y[1].To<TP2>())
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
        public IMapTask<T> Act<TP1, TP2, TP3>(Action<T, TP1, TP2, TP3> action, string alias1, string alias2, string alias3)
        {
            if (!string.IsNullOrWhiteSpace(alias1) && !string.IsNullOrWhiteSpace(alias2) && !string.IsNullOrWhiteSpace(alias3))
            {
                Put<TP1>(alias1);
                Put<TP2>(alias2);
                Put<TP3>(alias3);

                GroupActions.Add(new GroupAction
                {
                    Aliases = new List<string> { alias1, alias2, alias3 },
                    Action = (x, y) => action((T)x, y[0].To<TP1>(), y[1].To<TP2>(), y[2].To<TP3>())
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
        public IMapTask<T> Act<TP1, TP2, TP3, TP4>(Action<T, TP1, TP2, TP3, TP4> action, string alias1, string alias2, string alias3, string alias4)
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
                    Action = (x, y) => action((T)x, y[0].To<TP1>(), y[1].To<TP2>(), y[2].To<TP3>(), y[3].To<TP4>())
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
        public IMapTask<T> Act<TP1, TP2, TP3, TP4, TP5>(Action<T, TP1, TP2, TP3, TP4, TP5> action, string alias1, string alias2, string alias3, string alias4, string alias5)
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
                    Action = (x, y) => action((T)x, y[0].To<TP1>(), y[1].To<TP2>(), y[2].To<TP3>(), y[3].To<TP4>(), y[4].To<TP5>())
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
        public IMapTask<T> Act<TP1, TP2, TP3, TP4, TP5, TP6>(Action<T, TP1, TP2, TP3, TP4, TP5, TP6> action, string alias1, string alias2, string alias3, string alias4, string alias5, string alias6)
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
                    Action = (x, y) => action((T)x, y[0].To<TP1>(), y[1].To<TP2>(), y[2].To<TP3>(), y[3].To<TP4>(), y[4].To<TP5>(), y[5].To<TP6>())
                });
            }

            return this;
        }

        public IMapTask<T> Act(Action<IContent, T> action)
        {
            Action = (x, y) => action(x, (T)y);
            return this;
        }

        /// <summary>
        /// Map all public instance properties for the given object using naming convensions.
        /// </summary>
        /// <returns>This MapTask object.</returns>
        public new IMapTask<T> All()
        {
            base.All();
            return this;
        }

        /// <summary>
        /// To be used with All, to ignore a property doesn't need to be mapped.
        /// </summary>
        /// <typeparam name="TO">Object/Umbraco property type.</typeparam>
        /// <param name="property">Lambda expression for the object property, given the declaring object.</param>
        /// <returns></returns>
        public IMapTask<T> Ignore<TO>(Expression<Func<T, TO>> property)
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

        public IMapTask<T> Fit<TModel>(Expression<Func<T, TModel>> expModel, Func<TModel, string> funcKey = null, string alias = null)
            where TModel : class, new()
        {
            var action = expModel.ToSetter();

            if (string.IsNullOrWhiteSpace(alias))
                alias = expModel.ToInfo().Name;

            var modelMap = new ModelMap<T, TModel>(alias, action, funcKey) as IModelMap;

            ModelMaps.Add(modelMap);

            return this;
        }

        public IMapTask<T> Fit<TModel>(Expression<Func<T, IEnumerable<TModel>>> expModel, Func<TModel, string> funcKey = null, string alias = null)
            where TModel : class, new()
        {
            var action = expModel.ToSetter();

            if (string.IsNullOrWhiteSpace(alias))
                alias = expModel.ToInfo().Name;

            var modelMap = new ModelMap<T, TModel>(alias, action, funcKey);

            ModelMaps.Add(modelMap);

            return this;
        }

        public IMapTask<T> Fit<TModel>(Action<T, IEnumerable<TModel>> actionModel, string alias, Func<TModel, string> funcKey = null)
            where TModel : class, new()
        {
            if (!string.IsNullOrWhiteSpace(alias) && actionModel != null)
            {
                var modelMap = new ModelMap<T, TModel>(alias, actionModel, funcKey) as IModelMap;
                ModelMaps.Add(modelMap);
            }

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