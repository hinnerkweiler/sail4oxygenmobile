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
        return View();
    }
    
    [HttpGet("SoonFree")]
    public IActionResult SoonFree()
    {
        return View();
    }
    
    [HttpGet("Taken")]
    public IActionResult Taken()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult All()
    {
        return View();
    }
    
    
}