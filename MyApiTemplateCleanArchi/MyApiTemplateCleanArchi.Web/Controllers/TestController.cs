using Microsoft.AspNetCore.Mvc;

namespace MyApiTemplateCleanArchi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("fake-error")]
        public IActionResult GetFakeError()
        {
            throw new Exception("Ceci est une exception de test pour vérifier la journalisation.");
        }
    }
}
