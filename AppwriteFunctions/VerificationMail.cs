namespace DotNetRuntime;

using Appwrite;
using Appwrite.Services;
using Appwrite.Models;
using Appwrite.Extensions;

using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Collections;

public class Handler
{

    // This is your Appwrite function
    // It is executed each time we get a request
    public async Task<RuntimeOutput> Main(RuntimeContext Context)
    {

        var headers = JObject.Parse(Context.Req.Headers);

        //get user Id from Message
        //get user
        //create Verification
        //Send Mail to User

        return Context.Res.Empty();
    }
}