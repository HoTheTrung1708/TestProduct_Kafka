using Baitaptest.Memory;
using Microsoft.AspNetCore.Mvc;

namespace Baitaptest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly TableProductMemory _memory;


        public ProductController(TableProductMemory memory)
        {
            _memory = memory;
        }

        [HttpGet]
        public  IActionResult GetProduct()
        {
            var product = _memory.Memory.Values.ToList();
            return Ok(product);
        }
    }
}
