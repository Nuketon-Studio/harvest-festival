using System.Linq;

namespace HarvestFestival.Types
{
    public static class ProjectilePathTypes
    {
        // Watermelon
        public const string WATERMELON = "Projectiles/WatermelonProjectiilePrefab";
        public const string BANANA = "Projectiles/BananaProjectiilePrefab";

        public static string[] ToArray()
        {
            return typeof(ProjectilePathTypes)
            .GetFields()
            .Select(x => x.GetValue(null).ToString())
            .ToArray();
        }
    }
}