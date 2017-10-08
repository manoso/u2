using System;
using System.Collections.Generic;
using System.Linq;
using u2.Core.Contract;

namespace u2.Core
{
    public abstract class ModelMap : IModelMap
    {
        public static Func<object, string> DefaultGetKey = x => ((ICmsKey) x).Key;

        public Action<object, object> SetModel { get; set; }
        public Func<object, string> GetKey { get; set; }

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

        private IEnumerable<object> ToModels(IEnumerable<string> keys, IEnumerable<object> source, IEnumerable<object> empty = null)
        {
            if (keys == null || source == null || GetKey == null) return empty;
            return keys.Select(x => Find(x, source));
        }

        private object Find(string key, IEnumerable<object> source)
        {
            return source.FirstOrDefault(m => GetKey(m).Equals(key));
        }
    }

    public class ModelMap<T, TModel> : ModelMap
        where T : class, new()
        where TModel : class, new()
    {
        public ModelMap(string alias, Action<T, IEnumerable<TModel>> actionModel, Func<TModel, string> funcKey = null)
        {
            SetModel = (x, y) => actionModel((T)x, (IEnumerable<TModel>)y);
            Alias = alias.ToLowerInvariant();
            ModelType = typeof(TModel);
            IsMany = true;
            ToList = x => x.OfType<TModel>().ToList();
            GetKey = funcKey == null
                ? DefaultGetKey
                : x => funcKey((TModel)x);
        }

        public ModelMap(string alias, Action<T, TModel> actionModel, Func<TModel, string> funcKey = null)
        {
            SetModel = (x, y) => actionModel((T)x, (TModel)y);
            Alias = alias.ToLowerInvariant();
            ModelType = typeof(TModel);
            GetKey = funcKey == null
                ? DefaultGetKey
                : x => funcKey((TModel)x);
        }
    }

}