using System;
using System.Threading.Tasks;

public interface ITowerCharacteristicRepository
{
    Task<TowerCharacteristic> GetByIdAsync(Guid id);
}