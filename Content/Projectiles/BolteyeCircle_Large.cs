using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace myfirstmod.Content.Projectiles
{
    public class BolteyeCircle_Large : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Статичный спрайт
        }

        public override void SetDefaults()
        {
            Projectile.alpha = 255;

            Projectile.width = 25;
            Projectile.height = 91;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Если контроллер (BolteyeCircle) убит - убиваем этот круг
            bool controllerExists = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Projectile.owner && 
                    proj.type == ModContent.ProjectileType<BolteyeCircle>())
                {
                    controllerExists = true;
                    break;
                }
            }

            if (!controllerExists)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            Projectile.localAI[1]++;

            // Fade in
            float fadeDuration = 42f;
            if (Projectile.localAI[1] <= fadeDuration)
            {
                float progress = Projectile.localAI[1] / fadeDuration;
                Projectile.alpha = (int)(255f * (1f - progress));
                Projectile.scale = 0.7f + 0.3f * progress;
            }
            else
            {
                Projectile.alpha = 0;
                Projectile.scale = 1f;
            }

            // Пульсирующий эффект (более медленный чем у среднего)
            if (Projectile.localAI[1] >= 42)
            {
                float warpPulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.1f;
                Projectile.scale = 1f + warpPulse;

                // Редкие частички
                if (Main.rand.NextBool(4))
                {
                    Vector2 offset = Main.rand.NextVector2Circular(15f, 15f);
                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center + offset,
                        DustID.PurpleTorch,
                        Vector2.Zero
                    );
                    dust.noGravity = true;
                    dust.scale = 0.9f;
                    dust.velocity = offset * -0.03f;
                }
            }
        }
    }
}