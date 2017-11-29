namespace u2.Umbraco.Contract
{
    /// <summary>
    /// Umbraco configuration interface
    /// A implementation of this interface need to read the actual value from a configuration source like web.config or db etc.
    /// </summary>
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
