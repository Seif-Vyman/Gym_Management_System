using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextSeeding
    {
        public static bool SeedData(GymDbContext dbContext)
        {
            try
            {
                var HasPlans = dbContext.Plans.Any();
                var HasCategories = dbContext.Categorys.Any();
                if (HasCategories && HasPlans) return false;

                if (!HasPlans)
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (plans.Any())
                        dbContext.Plans.AddRange(plans);
                }
                if (!HasCategories)
                {
                    var categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (categories.Any())
                        dbContext.Categorys.AddRange(categories);
                }
                return dbContext.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding data failed : {ex}");
                return false;
            }
        }
        private static List<T> LoadDataFromJsonFile<T>(string fileName)
        {
            // F:\route_course\C#\5- MVC\GYM\GymManagmentSystemSolution\GymManagementPL\wwwroot\Files\categories.json

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);

            if (!File.Exists(filePath)) throw new FileNotFoundException();

            string data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? new List<T>();
        }
    }
}
