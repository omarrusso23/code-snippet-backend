using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SuperbaseServices;

[ApiController]
[Route("api/[controller]")]
public class CodeSnippetController : ControllerBase
{
    private readonly SuperbaseServices _supabaseService;

    public CodeSnippetController(SuperbaseServices supabaseService)
    {
        _supabaseService = supabaseService;
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveCodeSnippet([FromBody] CodeSnippet codeSnippet)
    {
        if (codeSnippet == null)
        {
            return BadRequest("Invalid code snippet.");
        }

        try
        {
            await _supabaseService.SaveCodeSnippetAsync(codeSnippet);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCodeSnippet(Guid id)
    {
        try
        {
            var snippet = await _supabaseService.GetCodeSnippetAsync(id);
            if (snippet == null)
            {
                return NotFound();
            }
            return Ok(snippet);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
