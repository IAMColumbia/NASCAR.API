using Nascar.Api.Entities;
using Nascar.Api.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Nascar.Api.DALs
{
    public class UserDAL
    {
        private string sqlConnectionString = string.Empty;
        private DatabaseConnectionSingleton connectionSingleton;

        public UserDAL()
        {
            connectionSingleton = DatabaseConnectionSingleton.Instance();
            sqlConnectionString = connectionSingleton.PrepareDBConnection();
        }

        public async Task<ResponseDto<User>> GetById(int id)
        {
            var response = new ResponseDto<User>();
            try
            {
                using(SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    using(SqlCommand sqlCommand = new SqlCommand("[dbo].[User_GetById]", conn))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@UserID", id).Direction = ParameterDirection.Input;

                        conn.Open();
                        var result =  await sqlCommand.ExecuteReaderAsync();
                        if(result.Read() == false)
                        {
                            throw new Exception($"User with id {id} was not found");
                        }
                        else
                        {
                            var test = new User() 
                            { 
                                ID = (int)result["ID"],
                                Username = (string)result["Username"],
                                Type = (string)result["Type"],
                                Avatar = (int)result["Avatar"]

                            };
                            response.Data = test;
                        }
                    }

                    conn.Close();
                }


            }
            catch (Exception ex)
            {
                if(ex.Message == $"User with id {id} was not found")
                {
                    throw;
                }
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ResponseDto<User>> GetByUsername(string username)
        {
            var response = new ResponseDto<User>();
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("[dbo].[User_GetByUsername]", conn))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Username", username).Direction = ParameterDirection.Input;

                        conn.Open();
                        var result = await sqlCommand.ExecuteReaderAsync();
                        if (result.Read() == false)
                        {
                            response.Data = null;
                        }
                        else
                        {
                            var test = new User()
                            {
                                ID = (int)result["ID"],
                                Username = (string)result["Username"],
                                Type = (string)result["Type"],
                                Avatar = (int)result["Avatar"]

                            };
                            response.Data = test;
                        }
                    }

                    conn.Close();
                }


            }
            catch (Exception ex)
            {
                if (ex.Message == $"User with username {username} was not found")
                {
                    throw;
                }
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ResponseDto<int>> Create(User newUser)
        {
            var response = new ResponseDto<int>();
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[User_Insert]", conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", newUser.Username).Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@TypeID", 1).Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@Avatar", newUser.Avatar).Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;

                        conn.Open();
                        await cmd.ExecuteNonQueryAsync();

                        response.Data = (int)cmd.Parameters["@ID"].Value;

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
