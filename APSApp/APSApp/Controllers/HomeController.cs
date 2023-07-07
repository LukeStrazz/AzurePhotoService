using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using APSApp.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace APSApp.Controllers;

public class HomeController : Controller
{
    private readonly IComputerVisionClient _computerVisionClient;

    public HomeController(IComputerVisionClient computerVisionClient)
    {
        _computerVisionClient = computerVisionClient;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Result()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AnalyzeImage(ImageUpload model)
    {
        if (ModelState.IsValid && model.IsValid)
        {
            using var ms = new MemoryStream();
            await model.ImageFile.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);

            ImageAnalysis analysis = await _computerVisionClient.AnalyzeImageInStreamAsync(ms);

            return View("Result", analysis);
        }

        if (!model.IsValid)
        {
            ModelState.AddModelError("ImageFile", "The file must be a .jpg, .jpeg, .png, .gif, .bmp and less than 5MB");
        }

        return View("Index", model);
    }

}


