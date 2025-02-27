using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NationalParksApi.Models;
using NationalParksApi.Data;
using Sprache;
using NationalParksApi.DTOs;

namespace NationalParksApi.Repositories
{
    public class DbParkRepository : IParkRepository
    {
        private readonly ParkContext _context;

        public DbParkRepository(ParkContext context)
        {
            _context = context;
        }

        public IEnumerable<NationalPark> GetAll()
        {
            return _context.Parks;
        }

        public PagedResponse<NationalPark> GetPagedParks(int pageNumber, int pageSize)
        {
            var totalRecords = _context.Parks.Count();

            var data = _context.Parks
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResponse<NationalPark>(data, pageNumber, pageSize, totalRecords);
        }

        public NationalPark? GetById(int id)
        {
            return _context.Parks.Find(id);
        }

        public NationalPark Add(NationalPark park)
        {
            _context.Parks.Add(park);
            _context.SaveChanges();
            return park;
        }

        public NationalPark? Update(int id, NationalPark updatedPark)
        {
            var existing = _context.Parks.Find(id);
            if (existing == null) return null;

            existing.Name = updatedPark.Name;
            existing.State = updatedPark.State;
            existing.YearEstablished = updatedPark.YearEstablished;
            existing.Latitude = updatedPark.Latitude;
            existing.Longitude = updatedPark.Longitude;
            _context.SaveChanges();
            return existing;
        }

        public bool Delete(int id)
        {
            var park = _context.Parks.Find(id);
            if (park == null) return false;

            _context.Parks.Remove(park);
            _context.SaveChanges();
            return true;
        }
    }
}
