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
        
        User user = JsonSerializer.Deserialize<User>(Environment.GetEnvironmentVariable("APPWRITE_FUNCTION_EVENT_DATA"));

        var pref = user.Prefs;

        Context.Log(pref.Data.ToString());

        

        //get user Id from Message
        //get user
        //create Verification
        //Send Mail to User

        return Context.Res.Empty();
    }
}