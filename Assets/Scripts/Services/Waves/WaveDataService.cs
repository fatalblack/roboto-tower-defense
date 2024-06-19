using System.Collections.Generic;
using System.Threading.Tasks;

public class WaveDataService : IWaveDataService
{
	private readonly IWaveRepository waveRepository;

	public WaveDataService(IWaveRepository waveRepository)
	{
		this.waveRepository = waveRepository;
	}

	public async Task<IEnumerable<Wave>> GetByWorldCodeAndStageNumberAsync(WorldCodes code, int stageNumber)
	{
		// return waves
		return await waveRepository.GetByWorldCodeAndStageNumberAsync(code, stageNumber);
	}
}