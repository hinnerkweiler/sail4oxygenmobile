using System;
using Appwrite;
using Appwrite.Services;

namespace Sailing4oxygenApi.Helpers
{
    public static class InitClient
    {
        public static Client AppwriteClient = new Client()
                .SetEndpoint(Helpers.Envs.Endpoint)
                .SetKey(Helpers.Envs.ApiKey)
                .SetProject(Helpers.Envs.Project);

        public static Databases databases = new Databases(Helpers.InitClient.AppwriteClient);
    }
}

