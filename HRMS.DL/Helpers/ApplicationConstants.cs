using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DL.Helpers
{
    public static class ApplicationConstants
    {
        public static string[] ListRoles = new string[] { "Admin" , "HR" , "Employee" };
        public static string[] ListBranch = new string[] { "Pune" , "Chennai" , "Noida" };
        public static string[] ListDesignation = new string[] { "HR", "Software Enginner" };
        public static string[] ListCertificateType = new string[] {
            "ID Cards-Default",
            "Year Completion Certificates-Default",
            "Spot Awards-Default",
            "Quarterly Awards-STAR,MVP",
            "Annual Awards-Cloud 9 Collaborators Award,Pinnacle Award,Champion Award,Excellence Award,Shining Star Award",
            "Whatever It Takes Award-Default",
            "Birthday Mail-Default",
            "Promotion Mail-Default",
            "Welcome Aboard Mail-Default",
            "New Born Baby-Default",
            "Wedding Mail-Default",
        };
        public static string[] ListFilelds = new string[] {
            "Employee Name-string",
            "Employee Id-string",
            "Contact Number-string",
            "Blood Group-Blood Group",
            "Photo-Photo",
            "Completed Year-Number List",
            "Current Month Year-Month Year",
            "Year-Year",
            "Team/Project Name-string",
            "Team Name-string",
            "Project Name-string",
        };

        public static string KeyError = "Error";
        public static string KeySuccess = "Success";

        public static string MessageInvallidLogin = "Login or Password is Incorrect";
        public static string MessageNotFound = "Not Found";
    }
}
