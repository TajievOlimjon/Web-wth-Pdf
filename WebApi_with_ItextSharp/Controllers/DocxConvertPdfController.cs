using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocxConvertPdfController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public DocxConvertPdfController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
