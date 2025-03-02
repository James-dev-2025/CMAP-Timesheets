
using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;

namespace Timesheets.Api.Services.CsvService
{
    public class CsvService : ICsvService
    {
        public byte[] GenerateCsv<T>(IEnumerable<T> records)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, new UTF8Encoding(false));
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

            csv.WriteRecords(records);
            writer.Flush();

            return memoryStream.ToArray();
        }
    }
}
