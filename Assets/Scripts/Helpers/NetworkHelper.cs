using System.Reflection.Emit;
using System.Threading.Tasks;
using Nakama.TinyJson;

namespace HarvestFestival.Helpers {
    static class NetworkHelper {
        public async static Task Send<T>(long opCode, T data) {
            await GameManager.Instance.Connection.Socket.SendMatchStateAsync(GameManager.Instance.matchManager.MatchId, opCode, JsonWriter.ToJson(data));
        }
    }
}