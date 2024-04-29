using Microsoft.AspNetCore.Mvc;
using Nascar.Api.DALs;
using Nascar.Api.Entities;
using Nascar.Api.Models;

namespace Nascar.Api.Controllers
{
    [Route("api/leaderboard")]
    [ApiController]
    public class LeaderboarController : ControllerBase
    {
        private LeaderboardDAL dal = new LeaderboardDAL();

        [HttpGet("{index}")]
        public async Task<ActionResult<ResponseDto<List<Leaderboard>>>> LeaderboardGetPaged(int index)
        {
            try
            {
                var result = await dal.GetPaged(index);
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
        public async Task<ActionResult<ResponseDto<bool>>> LeaderboardInsert([FromBody] LeaderboardRecord newRecord)
        {
            var result = await dal.InsertRecord(newRecord);
            if (result.Errors != null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
