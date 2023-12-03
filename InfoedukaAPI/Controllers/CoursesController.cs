using Dapper;
using InfoedukaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using InfoedukaAPI.Helpers;

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
        public async Task<ActionResult<IList<Course>>> GetAll(int userId)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("InfoedukaDB"));
            var response = 
                await conn.QueryAsync<Course>("exec dbo.spClassCmb @UserID=@UserID", new{ userID = userId});
            return Ok(response);
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateCourse(int appuser, string name, bool isActive)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("InfoedukaDB"));
            var sqlparam = new
            {
                AppUserID = appuser,
                ClassName = name,
                IsActive = BooleanConverter.BoolToBit(isActive)
            };

            var sqlexec 
                = "exec dbo.spClassC @AppUserID=@AppUserID,@ClassName=@ClassName,@IsActive=@IsActive";

            var exec = await conn.ExecuteAsync(sqlexec, sqlparam);

            if (exec > 0)
            {
                return Ok(exec);
            }
            else
            {
                return BadRequest(exec);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCourse(int appuser, int courseId)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("InfoedukaDB"));
            var sqlparam = new
            {
                AppUserID = appuser,
                ClassID = courseId
            };

            var sqlexec = "exec dbo.spClassD @AppUserID=@AppUserID,@CommentID=@CommentID";

            var exec = await conn.ExecuteAsync(sqlexec, sqlparam);

            if (exec > 0)
            {
                return Ok(exec);
            }
            else
            {
                return BadRequest(exec);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateComment(int appuser, int courseId,string name, bool isActive)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("InfoedukaDB"));
            var sqlparam = new
            {
                AppUserID = appuser,
                ClassID = courseId,
                ClassName = name,
                IsActive = BooleanConverter.BoolToBit(isActive)
            };

            var sqlexec = "exec dbo.spClassU @AppUserID=@AppUserID,@ClassID=@ClassID,@ClassName=@ClassName,@IsActive=@IsActive";

            var exec = await conn.ExecuteAsync(sqlexec, sqlparam);

            if (exec > 0)
            {
                return Ok(exec);
            }
            else
            {
                return BadRequest(exec);
            }
        }
    }
}