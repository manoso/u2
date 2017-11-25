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

        [Route("view")]
        public async Task<IHttpActionResult> GetView()
        {
            var views = await DataService.Get<View>();
            return Ok(views);
        }

        [Route("media")]
        public async Task<IHttpActionResult> GetMedia()
        {
            var views = await DataService.Get<Media>();
            return Ok(views);
        }
    }
}
