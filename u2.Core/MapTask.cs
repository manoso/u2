using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using u2.Core.Contract;
using u2.Core.Extensions;

namespace u2.Core
{
    public abstract class MapTask : BaseTask, IMapTask
    {
        private readonly OnceToken _once = new OnceToken();

        public string Alias { get; protected set; }

        public Type EntityType { get; protected set; }

        public IList<IModelMap> ModelMaps { get; } = new List<IModelMap>();

        public IDictionary<string, Type> CmsFields { get; } = new Dictionary<string, Type>();

        public IList<IGroupAction> GroupActions { get; protected set; } = new List<IGroupAction>();

        public Action<IContent, object> Action { get; protected set; }

        private ITaskDefer _taskDefer;
        public ITaskDefer TaskDefer
        {
            get
            {
                if (!ModelMaps.Any()) return null;
                if (_taskDefer == null)
                {
                    _once.Lock(() =>
                    {
                        _taskDefer = new TaskDefer(ModelMaps);
                    });
                }
                return _taskDefer;
            }
        }

        protected MapTask(Type type)
        {
            EntityType = type;
            Alias = type.Name.ToLowerInvariant();
        }

        public abstract object Create(object instance = null);

        public IMapTask MapAuto()
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

        public IMapTask<T> MapFunction<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, Func<IMapper, ICache, Task<object>>> mapFunc = null, TP defaultVal = default(TP))
        {
            var map = CreatItem(property, alias, mapFunc, defaultVal);
            if (map != null)
                AddMap(map);

            return this;
        }

        public IMapTask<T> MapAction<TP>(Action<T, TP> action, string alias)
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

        public IMapTask<T> MapAction<TP1, TP2>(Action<T, TP1, TP2> action, string alias1, string alias2)
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

        public IMapTask<T> MapAction<TP1, TP2, TP3>(Action<T, TP1, TP2, TP3> action, string alias1, string alias2, string alias3)
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

        public IMapTask<T> MapAction<TP1, TP2, TP3, TP4>(Action<T, TP1, TP2, TP3, TP4> action, string alias1, string alias2, string alias3, string alias4)
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

        public IMapTask<T> MapAction<TP1, TP2, TP3, TP4, TP5>(Action<T, TP1, TP2, TP3, TP4, TP5> action, string alias1, string alias2, string alias3, string alias4, string alias5)
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

        public IMapTask<T> MapAction<TP1, TP2, TP3, TP4, TP5, TP6>(Action<T, TP1, TP2, TP3, TP4, TP5, TP6> action, string alias1, string alias2, string alias3, string alias4, string alias5, string alias6)
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

        public IMapTask<T> MapContent(Action<IContent, T> action)
        {
            Action = (x, y) => action(x, (T)y);
            return this;
        }

        public new IMapTask<T> MapAuto()
        {
            base.MapAuto();
            return this;
        }

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

        public IMapTask<T> Match<TModel>(Expression<Func<T, TModel>> expModel, Func<TModel, string, bool> funcMatch = null, string alias = null)
            where TModel : class, new()
        {
            var action = expModel.ToSetter();

            if (string.IsNullOrWhiteSpace(alias))
                alias = expModel.ToInfo().Name;

            var modelMap = new ModelMap<T, TModel>(alias, action, funcMatch);

            ModelMaps.Add(modelMap);

            return this;
        }

        public IMapTask<T> MatchMany<TModel>(Expression<Func<T, IEnumerable<TModel>>> expModel, Func<TModel, string, bool> funcMatch = null, string alias = null)
            where TModel : class, new()
        {
            var action = expModel.ToSetter();

            if (string.IsNullOrWhiteSpace(alias))
                alias = expModel.ToInfo().Name;

            var modelMap = new ModelMap<T, TModel>(alias, action, funcMatch);

            ModelMaps.Add(modelMap);

            return this;
        }

        public IMapTask<T> MatchAction<TModel>(Action<T, IEnumerable<TModel>> actionModel, string alias = null, Func<TModel, string, bool> funcMatch = null)
            where TModel : class, new()
        {
            if (actionModel != null)
            {
                var modelMap = new ModelMap<T, TModel>(alias, actionModel, funcMatch) as IModelMap;
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