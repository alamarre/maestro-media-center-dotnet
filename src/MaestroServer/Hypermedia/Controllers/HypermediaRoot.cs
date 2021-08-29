using System;
using System.Collections.Generic;
using D2L.Hypermedia.Siren;
using MaestroServer.Hypermedia.Utilities;
using MaestroServer.Users;
using Microsoft.AspNetCore.Mvc;

namespace MaestroServer.Controllers
{
    [ApiController]
    [Route("")]
    public class HypermediaRoot : ControllerBase
    {
        private readonly CanonicalUrl canonicalUrl;
        private readonly IUserManager userManager;

        public HypermediaRoot(CanonicalUrl canonicalUrl, IUserManager userManager)
        {
            this.canonicalUrl = canonicalUrl;
            this.userManager = userManager;
        }

        [HttpGet]
        public SirenEntity Index()
        {
            List<ISirenAction> actions = new List<ISirenAction>();
            List<string> classes = new List<string>();
            List<ISirenLink> links = new List<ISirenLink> {
                canonicalUrl.GetCanonicalLink("/", "selfie")
            };
            if(userManager.GetCurrentUser != null)
            {
                links.Add(canonicalUrl.GetCanonicalLink($"users/{userManager.GetCurrentUser}", "user"));
            } else
            {
                classes.Add("anonymous");
            }

            return new SirenEntity(
                links: links,
                actions: actions,
                @class: classes.ToArray()
            );
        }
    }
}
