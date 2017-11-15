using System.Threading.Tasks;
using System.Web.Http;
using Ninject;
using u2.Demo.Service;

namespace u2.Demo.Api.Controllers.Api
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        [Inject]
        public IDataService DataService { get; set; }

        [Route("site")]
        public async Task<IHttpActionResult> GetSite()
        {
            var views = await DataService.GetViews();
            return Ok(views);
        }
    }
}
