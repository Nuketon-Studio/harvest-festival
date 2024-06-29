using System.Reflection.Emit;
using System.Threading.Tasks;
using Nakama.TinyJson;

namespace HarvestFestival.Helpers {
    static class CommunicationHelper {
        public async static Task Send<T>(long opCode, T data) {
            await GameManager.Instance.Connection.Socket.SendMatchStateAsync(GameManager.Instance.MatchId, opCode, JsonWriter.ToJson(data));
        }
    }
}