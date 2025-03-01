namespace Timesheets.Api.Services.CsvService
{
    public interface ICsvService
    {
        byte[] GenerateCsv<T>(IEnumerable<T> records);
    }
}
