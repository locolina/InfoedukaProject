using Dapper;
using InfoedukaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace InfoedukaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CoursesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Course>>> GetAll(int userID)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("InfoedukaDB"));
            var comments = 
                await conn.QueryAsync<vComment>("exec dbo.spClassCmb @UserID=@UserID", new{userID});
            return Ok(comments);
        }
    }
}