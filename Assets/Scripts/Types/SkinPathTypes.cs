using System.Linq;

namespace HarvestFestival.Types
{
    public static class SkinPathTypes
    {
        // Tester
        public const string TESTER_DEFAULT = "Skins/TestSkinDefaultPrefab";

        // Watermelon
        public const string WATERMELON_DEFAULT = "Skins/Watermelon/WatermelonSkinDefaultPrefab";
        public const string WATERMELON_GOLD = "Skins/Watermelon/WatermelonSkinGoldPrefab";

        // Banana        
        public const string BANANA_DEFAULT = "Skins/Banana/BananaSkinDefaultPrefab";

        public static string[] ToArray()
        {
            return typeof(SkinPathTypes)
            .GetFields()
            .Select(x => x.GetValue(null).ToString())
            .ToArray();
        }
    }
}