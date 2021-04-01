using API.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BaseApiController : ControllerBase
    {
        // protected readonly DataContext _context;

        // public BaseApiController(DataContext context)
        // {
        //     _context = context;
        // }
    }
}