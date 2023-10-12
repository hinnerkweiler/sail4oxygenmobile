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
        string trigger = Context.Req.Headers["x-appwrite-trigger"];
        string userFromHeader  = Context.Req.Headers["x-appwrite-event"];
        Console.WriteLine("trigger :   "+trigger+"\nuser :       "+userFromHeader);

        //get user Id from Message
        //get user
        //create Verification
        //Send Mail to User

        return Context.Res.Empty();
    }
}