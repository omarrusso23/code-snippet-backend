using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CodeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JDoodleController : ControllerBase
    {
        private readonly JDoodleCompilerService _jdoodleCompilerService;

        public JDoodleController(JDoodleCompilerService jdoodleCompilerService)
        {
            _jdoodleCompilerService = jdoodleCompilerService;
        }

        [HttpPost("compile")]
        public async Task<IActionResult> CompileCode([FromBody] CompileRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code) || string.IsNullOrWhiteSpace(request.Language))
            {
                return BadRequest("Code and language are required.");
            }

            try
            {
                var result = await _jdoodleCompilerService.CompileCodeAsync(request.Code, request.Language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class CompileRequest
    {
        public string Code { get; set; }
        public string Language { get; set; }
    }
}