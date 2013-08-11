using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;
using System.Data;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace O365_SharePointAutoHostedWeb.Pages
{
    public partial class Default : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        //Get Active SQL Connection
        protected SqlConnection GetActiveSqlConnection()
        {
            return new SqlConnection(GetCurrentConnectionString());
        }

        //Read connection string
        protected string GetCurrentConnectionString()
        {
            return WebConfigurationManager.AppSettings["SqlAzureConnectionString"];
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try { 
                //Clear the error text
                lblError.Text = "";

                //Create list object
                List<Employee> objListEmployee = new List<Employee>();

                //Get the employee name from text box
                string strEmployeeName = txtEmployeeName.Text;
                if (string.IsNullOrEmpty(strEmployeeName))
                {
                    lblError.Text = "Please enter employee first name";
                    return;
                }

                //Get SQL Connection
                using (SqlConnection sqlConn = GetActiveSqlConnection())
                using (SqlCommand sqlCmd = sqlConn.CreateCommand())
                {
                    sqlConn.Open();
                    sqlCmd.CommandText = String.Format("SELECT * FROM Employee WHERE FirstName = '{0}'", strEmployeeName);
                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //Fill data in list object
                                objListEmployee.Add(new Employee() {
                                    EmployeeName = reader["FirstName"].ToString(),
                                    MiddleName = reader["MiddleName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Email = reader["EmailAddress"].ToString(),
                                    Gender = reader["Gender"].ToString(),
                                    DepartmentName = reader["DepartmentName"].ToString()
                                });

                                gridViewEmployeeDetail.DataSource = objListEmployee; //Assign data to gridview
                                gridViewEmployeeDetail.DataBind();
                            }
                        }
                        else
                        {
                            lblError.Text = "No records found";
                            gridViewEmployeeDetail.DataSource = null; //Assign data to gridview
                            gridViewEmployeeDetail.DataBind();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }

    //Data contract to save data in list
    public class Employee
    {
        public string EmployeeName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DepartmentName { get; set; }
        public string Gender { get; set; }
    }
}