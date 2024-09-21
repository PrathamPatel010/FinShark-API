using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        public Task<List<Stock>> GetUserPortfolio(AppUser appUser);
        public Task<Portfolio> AddToPortfolio(Portfolio portfolio);
        public Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol);
    }
}