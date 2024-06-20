using System;
using System.Threading.Tasks;

public interface IWorldDataService
{
    Task<World> GetByIdAsync(Guid id);
}