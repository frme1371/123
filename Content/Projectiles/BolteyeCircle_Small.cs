using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;

namespace myfirstmod.Content.Projectiles
{
    public class BolteyeCircle_Small : ModProjectile
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

            // Когда круг полностью открылся
            if (Projectile.localAI[1] == 42)
            {
                // Вспышка света
                for (int i = 0; i < 15; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.BlueCrystalShard,
                        velocity
                    );
                    dust.noGravity = true;
                    dust.scale = 1.2f;
                }

                SoundEngine.PlaySound(SoundID.Item92 with { Volume = 0.8f }, Projectile.Center);
            }

            // Пульсирующий эффект
            if (Projectile.localAI[1] >= 42)
            {
                float warpPulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 5f) * 0.05f;
                Projectile.scale = 1f + warpPulse;

                // Искажение воздуха
                if (Main.rand.NextBool(3))
                {
                    Vector2 circle = Main.rand.NextVector2CircularEdge(25f, 25f);
                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center + circle,
                        DustID.Vortex,
                        circle.RotatedBy(MathHelper.PiOver2) * 0.05f
                    );
                    dust.noGravity = true;
                    dust.scale = 1f;
                }
            }

            // СТРЕЛЬБА - ОЧЕНЬ низкая скорость (раз в 180 тиков = 3 секунды)
            if (Projectile.localAI[1] >= 42 && Main.myPlayer == Projectile.owner)
            {
                if (Projectile.localAI[0]++ >= 180) // Очень низкая скорость стрельбы
                {
                    Projectile.localAI[0] = 0;

                    Vector2 direction = Main.MouseWorld - Projectile.Center;
                    direction.Normalize();

                    Vector2 shootVel = direction * 40f;
                    Vector2 shootPos = Projectile.Center + direction * 20f;

                    // Спавним снаряд (позже создадим альтернативный)
                    // Пока используем SightyBolt, потом заменим на новый
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        shootPos,
                        shootVel,
                        ModContent.ProjectileType<SightyBolt>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner
                    );
                }
            }
        }
    }
}