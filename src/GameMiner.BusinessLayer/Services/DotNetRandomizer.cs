using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMiner.BusinessLayer
{
    public class DotNetRandomizer : IRandomizationService
    {
        private static Random random;

        public DotNetRandomizer()
        {
            random = new Random(DateTime.UtcNow.Millisecond);
        }

        public int[] GenerateRandomIntegers(int min, int max, int numberOfValues)
        {
            int[] result = new int[numberOfValues];

            for (int i = 0; i < numberOfValues;)
            {
                int number = random.Next(min, max);

                if (!result.Any(n => n == number))
                {
                    result[i] = number;
                    i++;
                }
            }

            return result;
        }

        public Task<int[]> GenerateRandomIntegersAsync(int min, int max, int numberOfValues)
        {
            throw new NotImplementedException();
        }
    }
}
