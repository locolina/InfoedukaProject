using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using InfoedukaAPI.Helpers;
using InfoedukaAPI.Models;

namespace InfoedukaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AppUserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IList<AppUser>>> GetAll(int userId)
        {
            await using var conn = new SqlConnection(_configuration.GetConnectionString("InfoedukaDB"));
            var response =
                await conn.QueryAsync<AppUser>("exec dbo.spAppUserR @AppUserID=@AppUserID",
                    new { AppUserID = userId });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(int appUser, string firstName, string lastName, string userName, string pass, int userTypeId, bool isActive)
        {
            await using var conn = new SqlConnection(_configuration.GetConnectionString("InfoedukaDB"));
            var sqlparam = new
            {
                AppUserID = appUser,
                FirstName = firstName,
                LastName = lastName,
                UserName = userName.ToLower(),
                Pass = pass,
                UserTypeID = userTypeId,
                IsActive = BooleanConverter.BoolToBit(isActive)
            };

            var sqlexec
                = "exec dbo.spAppUserC @AppUserID=@AppUserID,@FirstName=@FirstName,@LastName=@LastName,@UserName=@UserName,@Pass=@Pass,@UserTypeID=@UserTypeID,@IsActive=@IsActive";

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
        public async Task<ActionResult> DeleteUser(int appUserId, int userId)
        {
            await using var conn = new SqlConnection(_configuration.GetConnectionString("InfoedukaDB"));
            var sqlparam = new
            {
                AppUserID = appUserId,
                UserID = userId
            };

            var sqlexec = "exec dbo.spAppUserD @AppUserID=@AppUserID,@UserID=@UserID";

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
        public async Task<ActionResult> UpdateUser(int appUserId, int userId,string firstName, string lastName, string userName, string pass, int userTypeId, bool isActive)
        {
            await using var conn = new SqlConnection(_configuration.GetConnectionString("InfoedukaDB"));
            var sqlparam = new
            {
                AppUserID = appUserId,
                UserID = userId,
                FirstName = firstName,
                LastName = lastName,
                UserName = userName.ToLower(),
                Pass = pass,
                UserTypeID = userTypeId,
                IsActive = BooleanConverter.BoolToBit(isActive)
            };

            var sqlexec =
                "exec dbo.spAppUserU @AppUserID=@AppUserID,@UserID=@UserID,@FirstName=@FirstName,@LastName=@LastName,@UserName=@UserName,@Pass=@Pass,@UserTypeID=@UserTypeID,@IsActive=@IsActive";

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