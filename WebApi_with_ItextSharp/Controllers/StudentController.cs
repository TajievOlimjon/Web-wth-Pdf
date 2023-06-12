using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public StudentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> Get()
        {
            var students = await _dbContext.Students.ToListAsync();
            return Ok(students);
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            return Ok(student);
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student student)
        {
            await _dbContext.Students.AddAsync(student);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0) return BadRequest("Data not added !");
            return Ok("Data successfully added !");
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Student model)
        {
            var student = await _dbContext.Students.FindAsync(model.Id);
            if (student == null) return NotFound("Data not found !");
            _dbContext.Students.Update(model);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0) return BadRequest("Data not updated !");
            return Ok("Data successfully updated !");
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            if (student == null) return NotFound("Data not found !");
            _dbContext.Students.Remove(student);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 0) return BadRequest("Data not removed !");
            return Ok("Data successfully removed !");
        }
    }
}
