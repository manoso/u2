namespace u2.Umbraco.Contract
{
    public interface IUmbracoConfig
    {
        /// <summary>
        /// The Examine search provider name used by u2. 
        /// See the ExamineSearchProviders section in ExamineSettings.config file of your Umbraco installation.
        /// Default is: ExternalSearcher
        /// </summary>
        string ExamineSearcher { get; }
    }
}
