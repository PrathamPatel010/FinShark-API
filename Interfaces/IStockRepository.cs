using api.DTOs.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        public Task<Stock> CreateAsync(Stock stockModel);
        public Task<List<Stock>> GetAllAsync(QueryObject query);
        public Task<Stock?> GetByIdAsync(int id);
        public Task<Stock?> UpdateAsync(int id, UpdateStockRequest updateStockDto);
        public Task<Stock?> DeleteAsync(int id);
        public Task<bool> IsStockExist(int id);
    }
}