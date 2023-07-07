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
        return Index();
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

            if (Request.IsAjaxRequest())
            {
                return PartialView("ResultPartial", analysis);
            }

            return View("Result", analysis);
        }

        if (!model.IsValid)
        {
            ModelState.AddModelError("ImageFile", "The file must be a .jpg, .jpeg, .png, .gif, .bmp and less than 5MB");
        }

        return View("Index", model);
    }

}

public static class HttpRequestExtensions
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}

