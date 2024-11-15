using csv_to_database.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace csv_to_database.Services
{
    public interface ICSVService
    {
        Task Convert(IFormFile file);
    }

    public class CSVService : ICSVService
    {
        private readonly string _connectionString;

        public CSVService(string configuration)
        {
            _connectionString = configuration;
        }

        public async Task Convert(IFormFile file)
        {
            try
            {
                var BasePriceCSVList = new List<BasePriceCSV>();
                var columnIndices = new Dictionary<string, int>();

                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var headerLine = await stream.ReadLineAsync();
                    if (headerLine != null)
                    {
                        headerLine = headerLine.StartsWith("\"") && headerLine.EndsWith("\"") ? headerLine.Trim('"') : headerLine;
                    }

                    if (headerLine != null)
                    {
                        var headers = headerLine.Split(',');

                        var properties = typeof(BasePriceCSV).GetProperties();

                        foreach (var property in properties)
                        {
                            int index = Array.IndexOf(headers, property.Name);
                            if (index != -1)
                            {
                                columnIndices[property.Name] = index;
                            }
                        }
                    }

                    string? line;
                    while ((line = await stream.ReadLineAsync()) != null)
                    {
                        line = line.StartsWith("\"") && line.EndsWith("\"") ? line.Trim('"') : line;
                        
                        var values = line.Split(',');


                        BasePriceCSVList.Add(new BasePriceCSV
                        {
                            Date = DateTime.ParseExact(values[columnIndices[nameof(BasePriceCSV.Date)]], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            MBP = double.Parse(values[columnIndices[nameof(BasePriceCSV.MBP)]], CultureInfo.InvariantCulture),
                            MBB = double.Parse(values[columnIndices[nameof(BasePriceCSV.MBB)]], CultureInfo.InvariantCulture),
                            CPAP = double.Parse(values[columnIndices[nameof(BasePriceCSV.CPAP)]], CultureInfo.InvariantCulture),
                            CPAB = double.Parse(values[columnIndices[nameof(BasePriceCSV.CPAB)]], CultureInfo.InvariantCulture),
                            AFP = double.Parse(values[columnIndices[nameof(BasePriceCSV.AFP)]], CultureInfo.InvariantCulture),
                            AFB = double.Parse(values[columnIndices[nameof(BasePriceCSV.AFB)]], CultureInfo.InvariantCulture),
                        });
                    }
                }

                var productCodes = new List<string> { "MBP", "MBB", "CPAP", "CPAB", "AFP", "AFB" };
                var productIdDict = await GetProductIdsByCodes(productCodes);

                List<ProductPriceDTO> productPriceDTOs = new List<ProductPriceDTO>();
                for (int i = 0; i < BasePriceCSVList.Count - 3; i++)
                {
                    var productIdMBP = productIdDict["MBP"];
                    var productIdMBB = productIdDict["MBB"];
                    var productIdCPAP = productIdDict["CPAP"];
                    var productIdCPAB = productIdDict["CPAB"];
                    var productIdAFP = productIdDict["AFP"];
                    var productIdAFB = productIdDict["AFB"];

                    // Proses MBPropane
                    productPriceDTOs.Add(new ProductPriceDTO()
                    {
                        ProductId = productIdMBP,
                        PriceDate = BasePriceCSVList[i].Date,
                        BasePrice = BasePriceCSVList[i].MBP,
                        RpaId = null, //No data from csv
                        PriceMonth1 = BasePriceCSVList[i + 1].MBP,
                        PriceMonth2 = BasePriceCSVList[i + 2].MBP,
                        PriceMonth3 = BasePriceCSVList[i + 3].MBP,
                        ChangeMonth1 = null, //No data from csv
                        ChangeMonth2 = null, //No data from csv
                        ChangeMonth3 = null, //No data from csv
                        Choice = 1, //Example value
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedAt = DateTime.UtcNow
                    });

                    // Proses MBButane
                    productPriceDTOs.Add(new ProductPriceDTO()
                    {
                        ProductId = productIdMBB,
                        PriceDate = BasePriceCSVList[i].Date,
                        BasePrice = BasePriceCSVList[i].MBB,
                        RpaId = null, //No data from csv
                        PriceMonth1 = BasePriceCSVList[i + 1].MBB,
                        PriceMonth2 = BasePriceCSVList[i + 2].MBB,
                        PriceMonth3 = BasePriceCSVList[i + 3].MBB,
                        ChangeMonth1 = null, //No data from csv
                        ChangeMonth2 = null, //No data from csv
                        ChangeMonth3 = null, //No data from csv
                        Choice = 1, //Example value
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedAt = DateTime.UtcNow
                    });

                    // Proses CPAPropane
                    productPriceDTOs.Add(new ProductPriceDTO()
                    {
                        ProductId = productIdCPAP,
                        PriceDate = BasePriceCSVList[i].Date,
                        BasePrice = BasePriceCSVList[i].CPAP,
                        RpaId = null, //No data from csv
                        PriceMonth1 = BasePriceCSVList[i + 1].CPAP,
                        PriceMonth2 = BasePriceCSVList[i + 2].CPAP,
                        PriceMonth3 = BasePriceCSVList[i + 3].CPAP,
                        ChangeMonth1 = null, //No data from csv
                        ChangeMonth2 = null, //No data from csv
                        ChangeMonth3 = null, //No data from csv
                        Choice = 1, //Example value
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedAt = DateTime.UtcNow
                    });

                    // Proses CPAButane
                    productPriceDTOs.Add(new ProductPriceDTO()
                    {
                        ProductId = productIdCPAB,
                        PriceDate = BasePriceCSVList[i].Date,
                        BasePrice = BasePriceCSVList[i].CPAB,
                        RpaId = null, //No data from csv
                        PriceMonth1 = BasePriceCSVList[i + 1].CPAB,
                        PriceMonth2 = BasePriceCSVList[i + 2].CPAB,
                        PriceMonth3 = BasePriceCSVList[i + 3].CPAB,
                        ChangeMonth1 = null, //No data from csv
                        ChangeMonth2 = null, //No data from csv
                        ChangeMonth3 = null, //No data from csv
                        Choice = 1, //Example value
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedAt = DateTime.UtcNow
                    });

                    // Proses AFEIPropane
                    productPriceDTOs.Add(new ProductPriceDTO()
                    {
                        ProductId = productIdAFP,
                        PriceDate = BasePriceCSVList[i].Date,
                        BasePrice = BasePriceCSVList[i].AFP,
                        RpaId = null, //No data from csv
                        PriceMonth1 = BasePriceCSVList[i + 1].AFP,
                        PriceMonth2 = BasePriceCSVList[i + 2].AFP,
                        PriceMonth3 = BasePriceCSVList[i + 3].AFP,
                        ChangeMonth1 = null, //No data from csv
                        ChangeMonth2 = null, //No data from csv
                        ChangeMonth3 = null, //No data from csv
                        Choice = 1, //Example value
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedAt = DateTime.UtcNow
                    });

                    // Proses AFEIButane
                    productPriceDTOs.Add(new ProductPriceDTO()
                    {
                        ProductId = productIdAFB,
                        PriceDate = BasePriceCSVList[i].Date,
                        BasePrice = BasePriceCSVList[i].AFB,
                        RpaId = null, //No data from csv
                        PriceMonth1 = BasePriceCSVList[i + 1].AFB,
                        PriceMonth2 = BasePriceCSVList[i + 2].AFB,
                        PriceMonth3 = BasePriceCSVList[i + 3].AFB,
                        ChangeMonth1 = null, //No data from csv
                        ChangeMonth2 = null, //No data from csv
                        ChangeMonth3 = null, //No data from csv
                        Choice = 1, //Example value
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false,
                        DeletedAt = DateTime.UtcNow
                    });
                }

                await SaveToDb(productPriceDTOs);
            }
            catch (Exception ex)
            {
                throw new Exception(message: ex.Message);
            }
        }

        public async Task<Dictionary<string, int>> GetProductIdsByCodes(List<string> codes)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT Id, Code FROM Product WHERE Code IN @Codes";
                var result = await connection.QueryAsync<ProductDTO>(query, new { Codes = codes });

                return result.ToDictionary(x => x.Code, x => x.Id);
            }
        }


        public async Task SaveToDb(List<ProductPriceDTO> productPriceDTOs)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                foreach (var dto in productPriceDTOs)
                {
                    var query = "INSERT INTO ProductPrice (ProductId, PriceDate, BasePrice, RpaId, PriceMonth1, PriceMonth2, PriceMonth3, ChangeMonth1, ChangeMonth2, ChangeMonth3, Choice, ConcurrencyStamp, CreatedAt, UpdatedAt, IsDeleted, DeletedAt) " +
                                "VALUES (@ProductId, @PriceDate, @BasePrice, @RpaId, @PriceMonth1, @PriceMonth2, @PriceMonth3, @ChangeMonth1, @ChangeMonth2, @ChangeMonth3, @Choice, @ConcurrencyStamp, @CreatedAt, @UpdatedAt, @IsDeleted, @DeletedAt)";

                    await connection.ExecuteAsync(query, dto);
                }
            }
        }
    }
}
