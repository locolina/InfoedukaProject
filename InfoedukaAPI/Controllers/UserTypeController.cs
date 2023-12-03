using InfoedukaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data.SqlClient;
using InfoedukaAPI.Helpers;

namespace InfoedukaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserTypeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserTypeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet]
        public async Task<ActionResult<IList<UserType>>> GetAll(int userId)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("InfoedukaDB"));
            var courses = 
                await conn.QueryAsync<UserType>("exec dbo.spUserTypeCmb @AppUserID=@AppUserID", 
                    new{ AppUserID = userId});
            return Ok(courses);
        }
    }
}