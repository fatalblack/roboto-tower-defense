using System.Collections.Generic;
using System.Threading.Tasks;

public interface IWaveRepository
{
    Task<IEnumerable<Wave>> GetByWorldCodeAndStageNumberAsync(WorldCodes code, int stageNumber);
}