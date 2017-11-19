using System;
using System.Collections.Generic;
using System.Linq;
using u2.Core.Contract;

namespace u2.Core
{
    public abstract class ModelMap : IModelMap
    {
        public static Func<object, string, bool> DefaultMatchKey = (model, key) => key.EndsWith(((ICmsKey)model).Key.ToString("N"));
        public static Func<object, string, bool> DefaultMatchId = (model, id) => id == ((ICmsModel<int>)model).Id.ToString();

        public Action<object, object> SetModel { get; set; }
        public Func<object, string, bool> IsMatch { get; set; }

        public string Alias { get; protected set; }
        public Type ModelType { get; protected set; }
        public bool IsMany { get; protected set; }

        protected Func<IEnumerable<object>, object> ToList { get; set; }

        public void Match(object target, IEnumerable<string> keys, IEnumerable<object> source)
        {
            var models = ToList(ToModels(keys, source));
            SetModel(target, models);
        }

        public void Match(object target, string key, IEnumerable<object> source)
        {
            var model = Find(key, source);
            SetModel(target, model);
        }

        public void Match(object target, IEnumerable<object> source)
        {
            SetModel(target, source);
        }

        private IEnumerable<object> ToModels(IEnumerable<string> keys, IEnumerable<object> source, IEnumerable<object> empty = null)
        {
            if (keys == null || source == null || IsMatch == null) return empty;
            return keys.Select(x => Find(x, source));
        }

        private object Find(string key, IEnumerable<object> source)
        {
            return source.FirstOrDefault(m => IsMatch(m, key));
        }
    }

    public class ModelMap<T, TModel> : ModelMap
        where T : class, new()
        where TModel : class, new()
    {
        public ModelMap(string alias, Action<T, IEnumerable<TModel>> actionModel, Func<TModel, string, bool> funcMatch = null)
        {
            SetModel = (x, y) => actionModel((T)x, (IEnumerable<TModel>)y);
            Alias = alias?.ToLowerInvariant();
            ModelType = typeof(TModel);
            IsMany = true;
            ToList = x => x.OfType<TModel>().ToList();
            IsMatch = funcMatch == null
                ? DefaultMatchKey
                : (model, key) => funcMatch((TModel)model, key);
        }

        public ModelMap(string alias, Action<T, TModel> actionModel, Func<TModel, string, bool> funcMatch = null)
        {
            SetModel = (x, y) => actionModel((T)x, (TModel)y);
            Alias = alias.ToLowerInvariant();
            ModelType = typeof(TModel);
            IsMatch = funcMatch == null
                ? DefaultMatchKey
                : (model, key) => funcMatch((TModel)model, key);
        }
    }

}