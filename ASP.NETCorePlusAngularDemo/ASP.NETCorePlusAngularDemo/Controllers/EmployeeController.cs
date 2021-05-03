using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using ASP.NETCorePlusAngularDemo.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ASP.NETCorePlusAngularDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT EmployeeId, EmployeeName, DepartmentName, DateOfJoining, PhotoFileName " +  
                            @"FROM dbo.Department JOIN dbo.Employee ON dbo.Employee.DepartmentId = dbo.Department.DepartmentId ;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
                return new JsonResult(table);
            }
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"INSERT INTO dbo.Employee(EmployeeName, DepartmentId, DateOfJoining, PhotoFileName) VALUES " +
                           @"(" + 
                               @"'" + emp.EmployeeName + @"', " +
                               emp.DepartmentId +
                               @", '" + emp.DateOfJoining + @"', " +
                               @"'" + emp.PhotoFileName + @"'" +
                           @")";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
                return new JsonResult("'" + emp.EmployeeName + "' Added Succesfuly To Employees");
            }
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"UPDATE dbo.Employee SET " +
                               @"EmployeeName = '" + emp.EmployeeName + @"'" +
                               @", dbo.Employee.DepartmentId = '" + emp.DepartmentId + @"'" +
                               @", DateOfJoining = '" + emp.DateOfJoining + @"'" +
                               @", PhotoFileName = '" + emp.PhotoFileName + @"'" +
                           @" WHERE EmployeeId = " + emp.EmployeeId;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
                return new JsonResult("Employee with ID: '" + emp.EmployeeId + "' is Updated Succesfuly.");
            }
        }

        [HttpDelete("{empName}")]
        public JsonResult Delete(String empName)
        {
            string query = @"DELETE FROM dbo.Employee WHERE EmployeeName = '" + empName + @"'";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
                return new JsonResult(empName + " is Deleted Succesfuly FROM Employees");
            }
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception e)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            string query = @"SELECT DepartmentName FROM dbo.Department";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
                return new JsonResult(table);
            }
        }
    }
}
