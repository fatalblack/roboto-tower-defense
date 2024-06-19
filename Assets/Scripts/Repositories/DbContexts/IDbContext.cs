using System.Threading.Tasks;

public interface IDbContext
{
	Task<DataContext> GetDataContextAsync();
}