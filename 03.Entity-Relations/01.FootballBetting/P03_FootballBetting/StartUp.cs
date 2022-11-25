using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data;

namespace P03_FootballBetting
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            FootballBettingContext dbContext = new FootballBettingContext();

            //dbContext.Database.EnsureCreated();

            //Console.WriteLine("Db created succesfully");
            //Console.WriteLine("Do you want to delete database (Y/N)");

            //string result = Console.ReadLine();
            // dbContext.Database.EnsureDeleted();

            dbContext.Database.Migrate();
        }
    }
}
