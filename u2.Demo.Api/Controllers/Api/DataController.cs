using System.Threading.Tasks;
using System.Web.Http;
using Ninject;
using u2.Demo.Data;
using u2.Demo.Service;
using u2.Umbraco.DataType.Media;

namespace u2.Demo.Api.Controllers.Api
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        [Inject]
        public IDataService DataService { get; set; }

        [Route("view/async")]
        public async Task<IHttpActionResult> GetViewAsync()
        {
            var views = await DataService.GetAsync<View>().ConfigureAwait(false);
            return Ok(views);
        }

        [Route("view")]
        public IHttpActionResult GetView()
        {
            var views = DataService.Get<View>();
            return Ok(views);
        }
    }
}
