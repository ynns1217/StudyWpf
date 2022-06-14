﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CnsLogTestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Commons.LOGGER.Info("DataBase 접속 시도");

			string connString = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True";

			string strQuery = @"SELECT Id
								     , EmpName
								     , Salary
								     , DeptName
								     , Destination
								 FROM  TblEmployees";

			Commons.LOGGER.Info("DataBase 설정 및 쿼리 작성");


			try
			{
				using (SqlConnection conn = new SqlConnection(connString))
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand(strQuery, conn);
					Commons.LOGGER.Warn("접속실패가 발생할 수 있습니다. 주의하세요!");

					SqlDataReader reader = cmd.ExecuteReader();

					while (reader.Read())
					{
						Console.WriteLine(reader["Id"]);
						Console.WriteLine(reader["EmpName"]);
						//Console.WriteLine(reader.GetString(0)); // log확인용 임의 오류발생 코드
					}
				}
			Commons.LOGGER.Info("DB 처리완료!");

			}
			catch (Exception ex)
			{

				Commons.LOGGER.Error($"예외발생! : {ex}");
				Console.WriteLine($"예외발생! : {ex}\n 관리자에게 문의하세요!!");
			}

			Commons.LOGGER.Info("DB 접속종료!");


		}
	}
}
