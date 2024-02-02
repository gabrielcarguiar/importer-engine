using System.Globalization;
using Importer.WApp.Data;
using Importer.WApp.Pages.Imports.Pokemons;

namespace Importer.WApp.Services
{
    public class MyPokemonService : IMyPokemonService
    {
        private readonly ApplicationDbContext _context;

        public MyPokemonService(ApplicationDbContext context)
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

        public async Task<IList<MyPokemonModel>> ImportMyPokemonFileAsync(byte[] file)
        {
            try
            {
                List<MyPokemonModel> myPokemons = new();

                using (var reader = new StreamReader(new MemoryStream(file)))
                using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<dynamic>();

                    foreach (var record in records.ToList())
                    {
                        var myPokemon = new MyPokemonModel(Convert.ToInt32(record.Id));

                        foreach (var property in record)
                        {
                            if (!nameof(MyPokemonModel.Id).Equals(property.Key, StringComparison.OrdinalIgnoreCase))
                            {
                                myPokemon.AddAditionalProp(property.Key, property.Value?.ToString());
                            }
                        }

                        myPokemons.Add(myPokemon);
                    }
                }

                return myPokemons;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }

    public interface IMyPokemonService
    {
        Task<IList<MyPokemonModel>> ImportMyPokemonFileAsync(byte[] file);

        Task<ImportModel> ImportFileAsync(IFormFile file);
    }
}
