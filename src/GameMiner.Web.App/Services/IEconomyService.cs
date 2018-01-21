namespace GameMiner.Web.App.Services
{
    public interface IEconomyService
    {
        long UpdateUserBalance(long userId);
        long GetHashesPerCredit();
    }
}