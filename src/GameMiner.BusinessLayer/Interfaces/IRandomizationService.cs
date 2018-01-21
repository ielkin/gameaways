using System;
using System.Threading.Tasks;

namespace GameMiner.BusinessLayer
{
    public interface IRandomizationService
    {
        int[] GenerateRandomIntegers(int min, int max, int numberOfValues);
        Task<int[]> GenerateRandomIntegersAsync(int min, int max, int numberOfValues);
    }
}