using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using u2.Core.Contract;
using u2.Demo.Data;
using u2.Demo.Service;
using u2.Fixture;

namespace u2.Demo.Api.Controllers.Api
{
    [RoutePrefix("api/other")]
    public class OtherController : ApiController
    {
        [Route("view/async")]
        public async Task<IHttpActionResult> GetViewAsync()
        {
            var views = await Context.Cache.FetchAsync<View>().ConfigureAwait(false);
            return Ok(views);
        }

        [Route("view")]
        public IHttpActionResult GetView()
        {
            var views = Context.Cache.Fetch<View>();
            return Ok(views);
        }
    }
}
