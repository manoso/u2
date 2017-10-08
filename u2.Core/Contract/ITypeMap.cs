using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace u2.Core.Contract
{
    public interface ITypeMap : ISimpleMap
    {
        Action<IContent, object> Action { get; }
        IDictionary<string, Type> CmsFields { get; }
        IList<IGroupAction> GroupActions { get; }
        IList<IModelMap> ModelMaps { get; }

        ITypeMap All();
        object Create(object instance);
    }

    public interface ITypeMap<T> : ITypeMap
        where T : class, new()
    {
        ITypeMap<T> Act(Action<IContent, T> action);
        ITypeMap<T> Act<TP>(Action<T, TP> action, string alias);
        ITypeMap<T> Act<TP1, TP2, TP3, TP4, TP5, TP6>(Action<T, TP1, TP2, TP3, TP4, TP5, TP6> action, string alias1, string alias2, string alias3, string alias4, string alias5, string alias6);
        ITypeMap<T> Act<TP1, TP2, TP3, TP4, TP5>(Action<T, TP1, TP2, TP3, TP4, TP5> action, string alias1, string alias2, string alias3, string alias4, string alias5);
        ITypeMap<T> Act<TP1, TP2, TP3, TP4>(Action<T, TP1, TP2, TP3, TP4> action, string alias1, string alias2, string alias3, string alias4);
        ITypeMap<T> Act<TP1, TP2, TP3>(Action<T, TP1, TP2, TP3> action, string alias1, string alias2, string alias3);
        ITypeMap<T> Act<TP1, TP2>(Action<T, TP1, TP2> action, string alias1, string alias2);
        ITypeMap<T> AliasTo(string alias);
        ITypeMap<T> Copy<TP>() where TP : class, new();
        ITypeMap<T> Fit<TModel>(Action<T, IEnumerable<TModel>> actionModel, string alias, Func<TModel, string> funcKey = null) where TModel : class, new();
        ITypeMap<T> Fit<TModel>(Expression<Func<T, IEnumerable<TModel>>> expModel, Func<TModel, string> funcKey = null, string alias = null) where TModel : class, new();
        ITypeMap<T> Fit<TModel>(Expression<Func<T, TModel>> expModel, Func<TModel, string> funcKey = null, string alias = null) where TModel : class, new();
        ITypeMap<T> Ignore<TO>(Expression<Func<T, TO>> property);
        ITypeMap<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP));
    }
}