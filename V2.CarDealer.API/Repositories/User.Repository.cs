using Dapper;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using V2.CarDealer.API.DTOs.UsersObjects;

namespace V2.CarDealer.API.UserRepository
{
    public static class CpfValidator
    {
        public static bool IsValid(string cpf)
        {
            if (cpf.Length != 11 || !cpf.All(char.IsDigit))
                return false;

            var invalidCpfs = new[] {
                "00000000000", "11111111111", "22222222222", "33333333333",
                "44444444444", "55555555555", "66666666666", "77777777777",
                "88888888888", "99999999999"
            };

            if (invalidCpfs.Contains(cpf))
                return false;

            var tempCpf = cpf.Substring(0, 9);
            var firstDigit = CalculateDigit(tempCpf);
            var secondDigit = CalculateDigit(tempCpf + firstDigit);

            return cpf.EndsWith(firstDigit.ToString() + secondDigit.ToString());
        }

        private static int CalculateDigit(string cpf)
        {
            var sum = 0;
            for (int i = 0; i < cpf.Length; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * (cpf.Length + 1 - i);
            }
            var result = sum % 11;
            return result < 2 ? 0 : 11 - result;
        }
    }

    public class UserRepositoryClass
    {
        public static User MakeLogin(Login login)
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    string query = "select users.id, users.email, users.name, users.birthdate, users.gender, users.cpf from users where users.email = @Email and users.password = @Password;";
                    User user = connection.Query<User>(query, new { Email = login.Email, Password = login.Password }).FirstOrDefault();
                    return user;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }

        public static string MakeRegister(Register user, DateTime birthdate)
        {
            if (!CpfValidator.IsValid(user.Cpf)) throw new ArgumentException("Invalid CPF.");

            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    string checkQuery = "SELECT COUNT(1) FROM users WHERE cpf = @Cpf OR email = @Email";
                    var existingCount = connection.ExecuteScalar<int>(checkQuery, new { Cpf = user.Cpf, Email = user.Email });

                    if (existingCount > 0)
                    {
                        throw new ArgumentException("CPF or Email already exists. Cannot insert duplicate key.");
                    }

                    var parameters = new
                    {
                        Email = user.Email,
                        Name = user.Name,
                        Password = user.Password,
                        Birthdate = birthdate,
                        Gender = user.Gender,
                        Cpf = user.Cpf
                    };

                    string query = "insert into users ( email, name, password, birthdate, gender, cpf ) values ( @Email, @Name, @Password, @Birthdate, @Gender, @Cpf );";
                    connection.Execute(query, parameters);

                    return "User registered successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
