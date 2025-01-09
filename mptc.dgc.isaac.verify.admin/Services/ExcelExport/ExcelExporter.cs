

namespace mptc.dgc.isaac.verify.admin.Services.ExcelExport
{
    public class ExcelExporter<T>
    {
        private readonly IExcelExportStrategy<T> _exportStrategy;

        public ExcelExporter(IExcelExportStrategy<T> exportStrategy)
        {
            _exportStrategy = exportStrategy;
        }
        public Task<MemoryStream> ExportAsync(List<T> data)
        {
            return _exportStrategy.ExportAsync(data);
        }
    }
}