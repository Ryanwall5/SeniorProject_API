using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeniorProject.Api.Models.Helper;

namespace SeniorProject.Api.Controllers
{
    [Route("api/")]
    public class RootController : Controller
    {
        private readonly IUrlHelper UrlHelper;

        public RootController(IUrlHelper urlHelper)
        {
            UrlHelper = urlHelper;
        }

        [HttpGet(Name = nameof(GetAllRootLinks))]
        public IActionResult GetAllRootLinks()
        {
            var response = new RootPages();
            response.Links.Add(new Link(UrlHelper.Link(nameof(GetAllRootLinks), null), "self", "GET"));
            response.Links.Add(new Link(UrlHelper.Link(nameof(ItemsController.GetAllItemsAsync), null),  "items_home", "GET"));

            return Ok(response);
        }

    }
}