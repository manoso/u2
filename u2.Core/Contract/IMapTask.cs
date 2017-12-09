using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    /// <summary>
    /// Metadata type to define a model type mapping from a CMS content.
    /// </summary>
    public interface IMapTask : IBaseTask
    {
        /// <summary>
        /// Get the defined content type alias.
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// Get the defined model type.
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        /// Get the map action defined to map the content to the model instance directly.
        /// </summary>
        Action<IContent, object> Action { get; }

        /// <summary>
        /// Get all content fields defined for map actions as dictionary.
        /// Field alias as the key and mapped to model property type as the value. 
        /// </summary>
        IDictionary<string, Type> CmsFields { get; }

        /// <summary>
        /// Get group actions defined.
        /// </summary>
        IList<IGroupAction> GroupActions { get; }

        /// <summary>
        /// Get model mappings defined to map from id field to cached model instance.
        /// </summary>
        IList<IModelMap> ModelMaps { get; }

        /// <summary>
        /// Get the defined task defer instance.
        /// </summary>
        ITaskDefer TaskDefer { get; }

        /// <summary>
        /// Fluent api, map all public instance properties from the model type to content fields using name matching.
        /// </summary>
        /// <returns>This MapTask object.</returns>
        IMapTask MapAuto();

        /// <summary>
        /// Return an instance of the model type.
        /// </summary>
        /// <param name="instance">An existing instance to use.
        /// Return it if the instance is not null and can be derived from the model type, otherwise return a new instance.
        /// </param>
        /// <returns></returns>
        object Create(object instance = null);
    }

    /// <summary>
    /// Generic metadata type to define a model type mapping from a CMS content.
    /// </summary>
    /// <typeparam name="T">The model type.</typeparam>
    public interface IMapTask<T> : IMapTask
        where T : class, new()
    {
        /// <summary>
        /// Fluent api, set the content type alias.
        /// </summary>
        /// <param name="alias">The content type alias.</param>
        /// <returns></returns>
        IMapTask<T> AliasTo(string alias);

        /// <summary>
        /// Fluent api, add a mapping action for the model type, map directly from the content to the model instance.
        /// </summary>
        /// <param name="action">The map action to do the model mapping.</param>
        /// <returns></returns>
        IMapTask<T> MapContent(Action<IContent, T> action);

        /// <summary>
        /// Fluent api, add a property mapping.
        /// </summary>
        /// <typeparam name="TP">The property type.</typeparam>
        /// <param name="property">Lambda expression of the model property.</param>
        /// <param name="alias">Mapping field alias for the property, if null, property name is used.</param>
        /// <param name="mapFunc">Mapping funcion to convert the field value to the property type.</param>
        /// <param name="defaultVal">Default value to use if field is not found or its value is empty.</param>
        /// <returns></returns>
        IMapTask<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP));

        /// <summary>
        /// Fluent api, add a property mapping that use other cached instances of other model types.
        /// </summary>
        /// <typeparam name="TP">The property type.</typeparam>
        /// <param name="property">Lambda expression of the model property.</param>
        /// <param name="alias">Mapping field alias for the property, if null, property name is used.</param>
        /// <param name="mapFunc">Mapping funcion that use other cached instances of other model types to convert the field value to the property type.</param>
        /// <param name="defaultVal">Default value to use if field is not found or its value is empty.</param>
        /// <returns></returns>
        IMapTask<T> MapFunction<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, Func<IMapper, ICache, Task<object>>> mapFunc = null, TP defaultVal = default(TP));

        /// <summary>
        /// Fluent api, add a mapping action for the model type using a CMS field.
        /// </summary>
        /// <typeparam name="TP">The type the field value is convert to.</typeparam>
        /// <param name="alias">Mapping field alias.</param>
        /// <param name="action">The mapping action using the field value to do the mapping.</param>
        /// <returns></returns>
        IMapTask<T> MapAction<TP>(Action<T, TP> action, string alias);

        /// <summary>
        /// Fluent api, add a mapping action for the model type using 2 CMS fields.
        /// </summary>
        /// <typeparam name="TP1">The type the 1st field value is convert to.</typeparam>
        /// <typeparam name="TP2">The type the 2nd field value is convert to.</typeparam>
        /// <param name="action">The mapping action using the 2 field values to do the mapping.</param>
        /// <param name="alias1">The 1st mapping field alias.</param>
        /// <param name="alias2">The 2nd mapping field alias.</param>
        /// <returns></returns>
        IMapTask<T> MapAction<TP1, TP2>(Action<T, TP1, TP2> action, string alias1, string alias2);

        /// <summary>
        /// Fluent api, add a mapping action for the model type using 3 CMS fields.
        /// </summary>
        /// <typeparam name="TP1">The type the 1st field value is convert to.</typeparam>
        /// <typeparam name="TP2">The type the 2nd field value is convert to.</typeparam>
        /// <typeparam name="TP3">The type the 3rd field value is convert to.</typeparam>
        /// <param name="action">The mapping action using the 3 field values to do the mapping.</param>
        /// <param name="alias1">The 1st mapping field alias.</param>
        /// <param name="alias2">The 2nd mapping field alias.</param>
        /// <param name="alias3">The 3rd mapping field alias.</param>
        /// <returns></returns>
        IMapTask<T> MapAction<TP1, TP2, TP3>(Action<T, TP1, TP2, TP3> action, string alias1, string alias2, string alias3);

        /// <summary>
        /// Fluent api, add a mapping action for the model type using 4 CMS fields.
        /// </summary>
        /// <typeparam name="TP1">The type the 1st field value is convert to.</typeparam>
        /// <typeparam name="TP2">The type the 2nd field value is convert to.</typeparam>
        /// <typeparam name="TP3">The type the 3rd field value is convert to.</typeparam>
        /// <typeparam name="TP4">The type the 4th field value is convert to.</typeparam>
        /// <param name="action">The mapping action using the 4 field values to do the mapping.</param>
        /// <param name="alias1">The 1st mapping field alias.</param>
        /// <param name="alias2">The 2nd mapping field alias.</param>
        /// <param name="alias3">The 3rd mapping field alias.</param>
        /// <param name="alias4">The 4th mapping field alias.</param>
        /// <returns></returns>
        IMapTask<T> MapAction<TP1, TP2, TP3, TP4>(Action<T, TP1, TP2, TP3, TP4> action, string alias1, string alias2, string alias3, string alias4);

        /// <summary>
        /// Fluent api, add a mapping action for the model type using 5 CMS fields.
        /// </summary>
        /// <typeparam name="TP1">The type the 1st field value is convert to.</typeparam>
        /// <typeparam name="TP2">The type the 2nd field value is convert to.</typeparam>
        /// <typeparam name="TP3">The type the 3rd field value is convert to.</typeparam>
        /// <typeparam name="TP4">The type the 4th field value is convert to.</typeparam>
        /// <typeparam name="TP5">The type the 5th field value is convert to.</typeparam>
        /// <param name="action">The mapping action using the 5 field values to do the mapping.</param>
        /// <param name="alias1">The 1st mapping field alias.</param>
        /// <param name="alias2">The 2nd mapping field alias.</param>
        /// <param name="alias3">The 3rd mapping field alias.</param>
        /// <param name="alias4">The 4th mapping field alias.</param>
        /// <param name="alias5">The 5th mapping field alias.</param>
        /// <returns></returns>
        IMapTask<T> MapAction<TP1, TP2, TP3, TP4, TP5>(Action<T, TP1, TP2, TP3, TP4, TP5> action, string alias1, string alias2, string alias3, string alias4, string alias5);

        /// <summary>
        /// Fluent api, add a mapping action for the model type using 6 CMS fields.
        /// </summary>
        /// <typeparam name="TP1">The type the 1st field value is convert to.</typeparam>
        /// <typeparam name="TP2">The type the 2nd field value is convert to.</typeparam>
        /// <typeparam name="TP3">The type the 3rd field value is convert to.</typeparam>
        /// <typeparam name="TP4">The type the 4th field value is convert to.</typeparam>
        /// <typeparam name="TP5">The type the 5th field value is convert to.</typeparam>
        /// <typeparam name="TP6">The type the 6th field value is convert to.</typeparam>
        /// <param name="action">The mapping action using the 6 field values to do the mapping.</param>
        /// <param name="alias1">The 1st mapping field alias.</param>
        /// <param name="alias2">The 2nd mapping field alias.</param>
        /// <param name="alias3">The 3rd mapping field alias.</param>
        /// <param name="alias4">The 4th mapping field alias.</param>
        /// <param name="alias5">The 5th mapping field alias.</param>
        /// <param name="alias6">The 6th mapping field alias.</param>
        /// <returns></returns>
        IMapTask<T> MapAction<TP1, TP2, TP3, TP4, TP5, TP6>(Action<T, TP1, TP2, TP3, TP4, TP5, TP6> action, string alias1, string alias2, string alias3, string alias4, string alias5, string alias6);

        /// <summary>
        /// Fluent api, add a match mapping using an action to set the model property.
        /// </summary>
        /// <typeparam name="TModel">The property model type. All property model instances are auto loaded from cache.</typeparam>
        /// <param name="actionModel">Action to set the model property. Given the model instance and matching property model instances.</param>
        /// <param name="alias">The mapping field alias. If null, property name is used.</param>
        /// <param name="funcMatch">The function to find the matching property model instance given the field string value.</param>
        /// <returns></returns>
        IMapTask<T> MatchAction<TModel>(Action<T, IEnumerable<TModel>> actionModel, string alias = null, Func<TModel, string, bool> funcMatch = null) where TModel : class, new();

        /// <summary>
        /// Fluent api, add a match mapping for a IEnumerable property.
        /// </summary>
        /// <typeparam name="TModel">The IEnumerable property item type.</typeparam>
        /// <param name="expModel">The IEnumerable property lambda expression.</param>
        /// <param name="funcMatch">The function to find the matching property model instance given the field string value.</param>
        /// <param name="alias">The mapping field alias. If null, property name is used.</param>
        /// <returns></returns>
        IMapTask<T> MatchMany<TModel>(Expression<Func<T, IEnumerable<TModel>>> expModel, Func<TModel, string, bool> funcMatch = null, string alias = null) where TModel : class, new();

        /// <summary>
        /// Fluent api, add a match mapping for a property.
        /// </summary>
        /// <typeparam name="TModel">The property type.</typeparam>
        /// <param name="expModel">The property lambda expression.</param>
        /// <param name="funcMatch">The function to find the matching property model instance given the field string value.</param>
        /// <param name="alias">The mapping field alias. If null, property name is used.</param>
        /// <returns></returns>
        IMapTask<T> Match<TModel>(Expression<Func<T, TModel>> expModel, Func<TModel, string, bool> funcMatch = null, string alias = null) where TModel : class, new();

        /// <summary>
        /// Fluent api, normally used with MapAuto, to remove a property mapping added previously.
        /// </summary>
        /// <typeparam name="TP">The property type.</typeparam>
        /// <param name="property">The lambda expression of the property to be removed.</param>
        /// <returns></returns>
        IMapTask<T> Ignore<TP>(Expression<Func<T, TP>> property);

        /// <summary>
        /// Fluent api, to remove a mapping added previously.
        /// </summary>
        /// <param name="alias">The field alias registered for the mapping.</param>
        /// <returns></returns>
        IMapTask<T> Ignore(string alias);
    }
}