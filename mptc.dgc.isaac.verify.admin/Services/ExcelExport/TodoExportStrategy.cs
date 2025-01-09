
using OfficeOpenXml;

namespace mptc.dgc.isaac.verify.admin.Services.ExcelExport
{
    public class TodoExportStrategy<T> : IExcelExportStrategy<T>
    {
        public Task<MemoryStream> ExportAsync(List<T> data)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add headers dynamically based on the properties of T
                var properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i].Name;
                }

                // Add data
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < properties.Length; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = properties[j].GetValue(data[i]);
                    }
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return Task.FromResult(stream);
            }
        }
    }

}