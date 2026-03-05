using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using myfirstmod.Content.Projectiles;

namespace myfirstmod.Content.Projectiles
{
    public class AltPortalController : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.friendly = false;
            Projectile.hostile = false;

            Projectile.hide = true; // невидимый
        }

public override void AI()
{
    Player player = Main.player[Projectile.owner];

    // Проверяем реальное состояние кнопки
    if (!Main.mouseRight)
    {
        Projectile.Kill();
        return;
    }

    Projectile.Center = player.Center;

    Projectile.localAI[0]++;
    int timer = (int)Projectile.localAI[0];

    if (timer == 30) SpawnPortal(0);
    if (timer == 90) SpawnPortal(1);
    if (timer == 180) SpawnPortal(2);
}

        private void SpawnPortal(int sizeType)
{
    if (Main.myPlayer != Projectile.owner)
        return;

    int projType = sizeType switch
    {
        0 => ModContent.ProjectileType<AltPortalSmall>(),
        1 => ModContent.ProjectileType<AltPortalMedium>(),
        _ => ModContent.ProjectileType<AltPortalLarge>()
    };

    Projectile.NewProjectile(
        Projectile.GetSource_FromThis(),
        Projectile.Center,
        Vector2.Zero,
        projType,
        Projectile.damage,
        Projectile.knockBack,
        Projectile.owner
    );
}
        
    }
}