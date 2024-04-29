using Nascar.Api.Entities;
using Nascar.Api.Models;
using System.Data.SqlClient;
using System.Data;

namespace Nascar.Api.DALs
{
    public class LeaderboardDAL
    {
        private string sqlConnectionString = string.Empty;
        private DatabaseConnectionSingleton connectionSingleton;

        public LeaderboardDAL()
        {
            connectionSingleton = DatabaseConnectionSingleton.Instance();
            sqlConnectionString = connectionSingleton.PrepareDBConnection();
        }

        public async Task<ResponseDto<List<Leaderboard>>> GetPaged(int index)
        {
            var response = new ResponseDto<List<Leaderboard>>();
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("[dbo].[Leaderboard_GetPaged]", conn))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@rowOffset", (index*5)).Direction = ParameterDirection.Input;

                        conn.Open();
                        var result = await sqlCommand.ExecuteReaderAsync();

                        var temp = new List<Leaderboard>();
                        while(result.Read())
                        {
                            var leaderboard = new Leaderboard()
                            {
                                ID = (int)result["ID"],
                                Score = (int)result["Score"],
                                Username = (string)result["Username"],
                                Type = (string)result["Type"],
                                Avatar = (int)result["Avatar"]

                            };
                            temp.Add(leaderboard);
                        }

                        response.Data = temp;
                    }

                    conn.Close();
                }


            }
            catch (Exception ex)
            {
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ResponseDto<bool>> InsertRecord(LeaderboardRecord record)
        {
            var response = new ResponseDto<bool>();
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Leaderboard_Insert]", conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Score", record.Score).Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@UserID", record.UserID).Direction = ParameterDirection.Input;

                        conn.Open();
                        await cmd.ExecuteNonQueryAsync();

                        conn.Close();
                    }

                }


            }
            catch (Exception ex)
            {
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
