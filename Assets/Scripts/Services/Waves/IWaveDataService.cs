using System.Collections.Generic;
using System.Threading.Tasks;

public interface IWaveDataService
{
    Task<IEnumerable<Wave>> GetByWorldCodeAndStageNumberAsync(WorldCodes code, int stageNumber);
}