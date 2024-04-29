using Microsoft.AspNetCore.Mvc;
using Nascar.Api.DALs;
using Nascar.Api.Entities;
using Nascar.Api.Models;

namespace Nascar.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserDAL dal = new UserDAL();

        [HttpGet("{username}")]
        public async Task<ActionResult<ResponseDto<User>>> UserGetByUsername(string username)
        {
            try
            {
                var result = await dal.GetByUsername(username);
                if (result.Errors != null)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = new ResponseDto<User>()
                {
                    Errors = new List<string> { ex.Message }
                };
                return NotFound(result);
            }


        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<int>>> UserCreate([FromBody] User newUser)
        {
                var result = await dal.Create(newUser);
                if (result.Errors != null)
                {
                    return BadRequest(result);
                }
                return Ok(result);
        }
    }
}
