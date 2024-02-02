using System.Globalization;
using Importer.WApp.Data;
using Importer.WApp.Pages.Imports.Films;

namespace Importer.WApp.Services
{
    public class MyFilmService : IMyFilmService
    {
        private readonly ApplicationDbContext _context;

        public MyFilmService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ImportModel> ImportFileAsync(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);

            byte[] fileBytes;

            using MemoryStream ms = new();
            file.CopyTo(ms);
            fileBytes = ms.ToArray();

            return new ImportModel(file.FileName, fileBytes);
        }

        public async Task<IList<MyFilmModel>> ImportMyFilmsFileAsync(byte[] file)
        {
            try
            {
                using var reader = new StreamReader(new MemoryStream(file));
                using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<MyFilmModel>().ToList();

                return records.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }

    public interface IMyFilmService
    {
        Task<IList<MyFilmModel>> ImportMyFilmsFileAsync(byte[] file);

        Task<ImportModel> ImportFileAsync(IFormFile file);
    }
}
