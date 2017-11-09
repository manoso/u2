using System.Web.Http;

namespace u2.Demo.Api.Controllers.Api
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        [Route("site")]
        public IHttpActionResult GetSite()
        {
            return Ok("australia");
        }
    }
}
