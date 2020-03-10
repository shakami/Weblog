using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;

namespace Weblog.API.Helpers
{
    public static class MediaTypes
    {
        public static bool IncludeLinks(string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                       out MediaTypeHeaderValue parsedMediaType))
            {
                return false;
            }

            return parsedMediaType
                    .SubTypeWithoutSuffix
                    .EndsWith("hateoas",
                          StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
