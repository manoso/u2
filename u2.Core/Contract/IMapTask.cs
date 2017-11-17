using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace u2.Core.Contract
{
    public interface IMapTask : IBaseTask
    {
        string Alias { get; }
        Type EntityType { get; }
        Action<IContent, object> Action { get; }
        IDictionary<string, Type> CmsFields { get; }
        IList<IGroupAction> GroupActions { get; }
        IList<IModelMap> ModelMaps { get; }

        /// <summary>
        /// Map all public instance properties for the given object using naming convensions.
        /// </summary>
        /// <returns>This MapTask object.</returns>
        IMapTask All();

        object Create(object instance = null);
    }

    public interface IMapTask<T> : IMapTask
        where T : class, new()
    {
        IMapTask<T> Act(Action<IContent, T> action);

        /// <summary>
        /// Map a CMS property to object properties using Action.
        /// </summary>
        /// <typeparam name="TP">CMS property type.</typeparam>
        /// <param name="alias">CMS property alias.</param>
        /// <param name="action">Run to do the mapping.</param>
        /// <returns></returns>
        IMapTask<T> Act<TP>(Action<T, TP> action, string alias);
        IMapTask<T> Act<TP1, TP2>(Action<T, TP1, TP2> action, string alias1, string alias2);
        IMapTask<T> Act<TP1, TP2, TP3>(Action<T, TP1, TP2, TP3> action, string alias1, string alias2, string alias3);
        IMapTask<T> Act<TP1, TP2, TP3, TP4>(Action<T, TP1, TP2, TP3, TP4> action, string alias1, string alias2, string alias3, string alias4);
        IMapTask<T> Act<TP1, TP2, TP3, TP4, TP5>(Action<T, TP1, TP2, TP3, TP4, TP5> action, string alias1, string alias2, string alias3, string alias4, string alias5);
        IMapTask<T> Act<TP1, TP2, TP3, TP4, TP5, TP6>(Action<T, TP1, TP2, TP3, TP4, TP5, TP6> action, string alias1, string alias2, string alias3, string alias4, string alias5, string alias6);
        IMapTask<T> AliasTo(string alias);
        IMapTask<T> Fit<TModel>(Action<T, IEnumerable<TModel>> actionModel, string alias, Func<TModel, string, bool> funcMatch = null) where TModel : class, new();
        IMapTask<T> Fit<TModel>(Expression<Func<T, IEnumerable<TModel>>> expModel, Func<TModel, string, bool> funcMatch = null, string alias = null) where TModel : class, new();
        IMapTask<T> Fit<TModel>(Expression<Func<T, TModel>> expModel, Func<TModel, string, bool> funcMatch = null, string alias = null) where TModel : class, new();
        /// <summary>
        /// To be used with All, to ignore a property doesn't need to be mapped.
        /// </summary>
        /// <typeparam name="TO">Object/Umbraco property type.</typeparam>
        /// <param name="property">Lambda expression for the object property, given the declaring object.</param>
        /// <returns></returns>
        IMapTask<T> Ignore<TO>(Expression<Func<T, TO>> property);
        IMapTask<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP));
    }
}