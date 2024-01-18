using BaxtureAssignAuthAPI.Models;
using OfficeOpenXml;
using System.Reflection.Metadata;

namespace BaxtureAssignAuthAPI.HelperClass
{
    public static class ExportHelper
    {
        public static byte[] ExportUsers(List<User> users)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Users");

                // Add header row
                worksheet.Cells["A1"].Value = "User ID";
                worksheet.Cells["B1"].Value = "Username";
                worksheet.Cells["C1"].Value = "Age";

                // Add data to the Excel sheet based on users list
                int row = 2; // Start from the second row for data
                foreach (var user in users)
                {
                    worksheet.Cells[$"A{row}"].Value = user.Id;
                    worksheet.Cells[$"B{row}"].Value = user.Username;
                    worksheet.Cells[$"C{row}"].Value = user.Age;
                    // Add more columns as needed
                    row++;
                }

                return package.GetAsByteArray();
            }

        }
    }

}
