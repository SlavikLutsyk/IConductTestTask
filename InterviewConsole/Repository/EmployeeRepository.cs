using EmployeeService.Implementation.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Implementation.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["EmployeeDb"].ConnectionString;

        public async Task<IEnumerable<EmployeeDto>> GetAllActiveEmployeesAsync(CancellationToken cancellationToken)
        {
            var employees = new List<EmployeeDto>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT ID, Name, ManagerID FROM Employee WHERE Enable = 1";

                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync(cancellationToken);

                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            employees.Add(new EmployeeDto
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                ManagerId = reader["ManagerID"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["ManagerID"])
                            });
                        }
                    }
                }
            }

            return employees;
        }

        public async Task<EmployeeDto> UpdateEnableEmployeeAsync(int id, int enable, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = new SqlCommand("update_enable_employee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@Enable", SqlDbType.Bit).Value = enable;

                    await connection.OpenAsync(cancellationToken);

                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (await reader.ReadAsync(cancellationToken))
                        {
                            return new EmployeeDto
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                ManagerId = reader["ManagerID"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["ManagerID"]),
                                Enabled = Convert.ToBoolean(reader["Enable"]),
                                Employees = null
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
