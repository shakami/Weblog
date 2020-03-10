using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weblog.API.Models;
using Weblog.API.ResourceParameters;

namespace Weblog.API.Helpers
{
    public delegate string CreateResourceUri(
            int[] ids,
            ResourceParametersBase resourceParameters,
            ResourceUriType type);

    public static class LinksForCollection
    {
        public static List<LinkDto> Create(
            CreateResourceUri createResourceUri,
            int[] ids,
            ResourceParametersBase resourceParameters,
            bool hasPrevious,
            bool hasNext)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(createResourceUri(
                                    ids,
                                    resourceParameters,
                                    ResourceUriType.Current),
                                  "self",
                                  HttpMethods.Get)
            };

            if (hasPrevious)
            {
                links.Add(new LinkDto(createResourceUri(
                                        ids,
                                        resourceParameters,
                                        ResourceUriType.PreviousPage),
                                      "previousPage",
                                      HttpMethods.Get));
            }

            if (hasNext)
            {
                links.Add(new LinkDto(createResourceUri(
                                        ids,
                                        resourceParameters,
                                        ResourceUriType.NextPage),
                                      "nextPage",
                                      HttpMethods.Get));
            }

            return links;
        }
    }
}
