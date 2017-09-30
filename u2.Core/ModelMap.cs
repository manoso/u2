using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using u2.Core.Contract;
using u2.Core.Extensions;

namespace u2.Core
{
    public abstract class ModelMap
    {
        public static Func<object, string> DefaultGetKey = x => ((ICmsKey) x).Key;

        public Action<object, object> SetModel { get; set; }
        public Func<object, string> GetKey { get; set; }

        public string Alias { get; protected set; }
        public Type ModelType { get; protected set; }

        protected Func<IEnumerable<object>, object> ToList { get; set; }

        public void Match(object target, IEnumerable<string> keys, IEnumerable<object> source)
        {
            var models = ToList(ToModels(keys, source, GetKey));
            SetModel(target, models);
        }

        private static IEnumerable<object> ToModels(IEnumerable<string> keys, IEnumerable<object> source, Func<object, string> getKey, IEnumerable<object> empty = null)
        {
            if (keys == null || source == null || getKey == null) return empty;
            return keys.Select(x => source.FirstOrDefault(m => getKey(m).Equals(x)));
        }
    }

    public class ModelMap<T, TModel> : ModelMap
        where T : class, new()
        where TModel : class, new()
    {
        public ModelMap(string alias, Action<T, IEnumerable<TModel>> actionModel)
        {
            SetModel = (x, y) => actionModel((T) x, (IEnumerable<TModel>) y);
            Alias = (alias ?? typeof(TModel).Name).ToLowerInvariant();
            ModelType = typeof(TModel);
            ToList = x => x.OfType<TModel>().ToList();
            GetKey = DefaultGetKey;
        }
    }

    public class ModelMap<T, TModel, TKey> : ModelMap<T, TModel>
        where T : class, new()
        where TModel : class, new()
    {
        public ModelMap(string alias, Action<T, IEnumerable<TModel>> actionModel, Func<TModel, TKey> funcKey)
            : base(alias, actionModel)
        {
            GetKey = x => funcKey((TModel)x).ToString();
        }
    }
}