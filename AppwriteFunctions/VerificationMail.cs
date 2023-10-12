namespace DotNetRuntime;

using Appwrite;
using Appwrite.Services;
using Appwrite.Models;
using Appwrite.Extensions;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Collections;
using System.Text.RegularExpressions;

public class Handler
{

    // This is your Appwrite function
    // It is executed each time we get a request
    public async Task<RuntimeOutput> Main(RuntimeContext Context)
    {
        try
        {
            string trigger = Context.Req.Headers["x-appwrite-trigger"];

            string userFromHeader = Context.Req.Headers["x-appwrite-event"];

            string pattern = @"(?<=user\.)[^.]+(?=\.create)";

            Match match = Regex.Match(userFromHeader, pattern);

            Users users = new(DotNetRuntime.Helpers.InitClient.AppwriteClient);

            User user = await users.Get(match.Value.ToString());

            Context.Log("Verification" + user.EmailVerification);
        }
        catch (Exception ex)
        {
            Context.Log(ex.Message);
        }
        

        //get user
        //create Verification
        //Send Mail to User


        return Context.Res.Empty();
    }
}