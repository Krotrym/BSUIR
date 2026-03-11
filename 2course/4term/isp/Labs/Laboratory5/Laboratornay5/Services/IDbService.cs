using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratornay5.Services
{
    public interface IDbService
    {
        IEnumerable<CarBrand> GetAllCars();
        IEnumerable<Annoucement> GetCarsMembers(int id);

        void Init();
    }
}
