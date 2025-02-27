using System.Collections.Generic;
using NationalParksApi.DTOs;
using NationalParksApi.Models;

namespace NationalParksApi.Repositories
{
    public interface IParkRepository
    {
        IEnumerable<NationalPark> GetAll();
        PagedResponse<NationalPark> GetPagedParks(int pageNumber, int pageSize);
        NationalPark? GetById(int id);
        NationalPark Add(NationalPark park);
        NationalPark? Update(int id, NationalPark park);
        bool Delete(int id);
    }
}