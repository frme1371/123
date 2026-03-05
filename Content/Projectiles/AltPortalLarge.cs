using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace myfirstmod.Content.Projectiles
{
    public class AltPortalLarge : ModProjectile
    {
        public override void SetDefaults()
        {

            Projectile.alpha = 255; // полностью прозрачный при спавне
            
            Projectile.width = 25;
            Projectile.height = 91;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;

            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        public override void AI()
{
    Player player = Main.player[Projectile.owner];

    // Если ПКМ отпущена — исчезаем
    if (!player.channel || player.altFunctionUse != 2)
    {
        Projectile.Kill();
        return;
    }

    // Позиция — центр игрока
    Projectile.Center = player.Center;

    // Поворот к курсору
    Vector2 direction = Main.MouseWorld - player.Center;
    Projectile.rotation = direction.ToRotation();

    // Плавное появление
    if (Projectile.alpha > 0)
        Projectile.alpha -= 15;

    if (Projectile.alpha < 0)
        Projectile.alpha = 0;
}
    }
}