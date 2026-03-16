using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Laboratornay5.Services
{
    class SQLiteService : IDbService
    {
        //для тех кто взял код из гитхаба: ниже в "" находиться файл в которой находится
        //наша база данных поэтому так как я ленивая жопа чтобы всё красиво работало
        //откроейте это файл в блокноте и стерите всё содержимое
        static string pathToDb = @"D:\2kyrs\MyDataBase.db3";
        

        SQLiteConnection db = new SQLiteConnection(pathToDb);
        public void Init()
        {
            db.CreateTable<Annoucement>();
            db.CreateTable<CarBrand>();

            db.InsertAll(new List<CarBrand>
            { 
                new CarBrand { Id = 1, Name = "Mersedes"},
                new CarBrand { Id = 2, Name = "BMW"},
                new CarBrand { Id = 3, Name = "Tesla" }
            });

            db.InsertAll(new List<Annoucement>
            {
                new Annoucement {Title = "I will sell a Mercedes for 1$" , CarId = 1},
                new Annoucement {Title = "I will sell a Mercedes for 2$" , CarId = 1},
                new Annoucement {Title = "I will sell a Mercedes for 3$" , CarId = 1},
                new Annoucement {Title = "I will sell a Mercedes for 4$" , CarId = 1},
                new Annoucement {Title = "I will sell a Mercedes for 5$" , CarId = 1},
                new Annoucement {Title = "I will sell a Mercedes for 6$" , CarId = 1},
                new Annoucement {Title = "I will sell a Mercedes for 7$" , CarId = 1},
                
                new Annoucement {Title = "I will sell a BMW for 1$" , CarId = 2},
                new Annoucement {Title = "I will sell a BMW for 2$" , CarId = 2},
                new Annoucement {Title = "I will sell a BMW for 3$" , CarId = 2},
                new Annoucement {Title = "I will sell a BMW for 4$" , CarId = 2},
                new Annoucement {Title = "I will sell a BMW for 5$" , CarId = 2},
                new Annoucement {Title = "I will sell a BMW for 6$" , CarId = 2},
                new Annoucement {Title = "I will sell a BMW for 7$" , CarId = 2},

                new Annoucement {Title = "I will sell a Tesla for 1$" , CarId = 3},
                new Annoucement {Title = "I will sell a Tesla for 2$" , CarId = 3},
                new Annoucement {Title = "I will sell a Tesla for 3$" , CarId = 3},
                new Annoucement {Title = "I will sell a Tesla for 4$" , CarId = 3},
                new Annoucement {Title = "I will sell a Tesla for 5$" , CarId = 3},
                new Annoucement {Title = "I will sell a Tesla for 6$" , CarId = 3},
                new Annoucement {Title = "I will sell a Tesla for 7$" , CarId = 3},
            });
        }

        public IEnumerable<Annoucement> GetCarsMembers(int id)
        {
            var items = db.Table<Annoucement>()
                        .Where(i => i.CarId == id)
                        .ToList();

            return items;
        }

        public IEnumerable<CarBrand> GetAllCars()
        {
            var items = db.Table<CarBrand>().ToList();
            return items;
        }
    }
}
