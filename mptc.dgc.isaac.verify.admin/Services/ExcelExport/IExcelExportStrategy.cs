namespace mptc.dgc.isaac.verify.admin.Services.ExcelExport
{
    public interface IExcelExportStrategy<T>
    {
        Task<MemoryStream> ExportAsync(List<T> data);
    }
}