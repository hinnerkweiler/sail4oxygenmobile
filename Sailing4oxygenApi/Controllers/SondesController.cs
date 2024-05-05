using Microsoft.AspNetCore.Mvc;

namespace Sailing4oxygenApi.Controllers;

/// <summary>
/// Returns GEOJson data for Sondes. 
/// </summary>
[Route("[controller]")]
public class SondesController : Controller
{
    [HttpGet("Free")]
    public IActionResult Free()
    {
        return Ok("Free Sondes");
    }
    
    [HttpGet("SoonFree")]
    public IActionResult SoonFree()
    {
        return Ok("Soon Free Sondes");
    }
    
    [HttpGet("Taken")]
    public IActionResult Taken()
    {
        return Ok("Taken Sondes");
    }
    
    [HttpGet]
    public IActionResult All()
    {
        return Ok("All Sondes");
    }
    
    
}