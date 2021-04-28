using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace BoInsurance.Services
{
    public class MattrVerifiedSuccessHub : Hub
    {
        /// <summary>
        /// This should be replaced with a cache which expires or something
        /// </summary>
        public static readonly ConcurrentDictionary<string, string> Challenges = new ConcurrentDictionary<string, string>();

        public void AddChallenge(string challengeId, string connnectionId)
        {
            Challenges.TryAdd(challengeId, connnectionId);
        }

    }
}
