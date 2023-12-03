using Dapper;
using InfoedukaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace InfoedukaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CommentController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<IList<vComment>>> GetAll()
        {
            using var conn = new SqlConnection(_config.GetConnectionString("InfoedukaDB"));
            var comments = await conn.QueryAsync<vComment>("SELECT * FROM dbo.vComment");
            return Ok(comments);
        }

        [HttpPost]
        public async Task<ActionResult> PostComment(int appuser, string title, string content, DateTime? endDate, int isActive, int userid, int classid)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("InfoedukaDB"));
            var sqlparam = new
            {
                AppUserID = appuser,
                Title = title,
                Content = content,
                DateExpires = endDate,
                IsActive = isActive,
                UserID = userid,
                Classid = classid
            };

            var sqlexec = "exec dbo.spCommentC @AppUserID=@AppUserID,@Title=@Title,@Content=@Content,@DateExpires=@DateExpires,@IsActive=@IsActive,@UserID=@UserID,@ClassID=@ClassID";

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
        public async Task<ActionResult> DeleteComment(int appuser, int commentID)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("InfoedukaDB"));
            var sqlparam = new
            {
                AppUserID = appuser,
                CommentID = commentID
            };

            var sqlexec = "exec dbo.spCommentD @AppUserID=@AppUserID,@CommentID=@CommentID";

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
        public async Task<ActionResult> UpdateComment(int commentID, int appuser, string title, string content, DateTime? endDate, int isActive, int userid, int classid)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("InfoedukaDB"));
            var sqlparam = new
            {
                CommentID = commentID,
                AppUserID = appuser,
                Title = title,
                Content = content,
                DateExpires = endDate,
                IsActive = isActive,
                UserID = userid,
                Classid = classid
            };

            var sqlexec = "exec dbo.spCommentU @CommentID=@CommentID, @AppUserID=@AppUserID,@Title=@Title,@Content=@Content,@DateExpires=@DateExpires,@IsActive=@IsActive,@UserID=@UserID,@ClassID=@ClassID";

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
