using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;
using myfirstmod.Content.Items;

namespace myfirstmod.Content.Projectiles
{
    public class BolteyePortal : ModProjectile
    {
        public override Color? GetAlpha(Color lightColor)
        {
            float pulse = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);

            Color cosmic = new Color(120, 80, 255);
            Color arcane = new Color(60, 200, 255);

            Color finalColor = Color.Lerp(cosmic, arcane, pulse);

            return finalColor * (1f - Projectile.alpha / 255f);
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6; // твои кадры
        }

        public override void SetDefaults()
        {
            Projectile.alpha = 255; // старт невидим

            Projectile.width = 20;
            Projectile.height = 52;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            
        }

        public override void AI()
{
    if (Projectile.localAI[1] == 1)
    {
        SoundEngine.PlaySound(SoundID.Item122 with { Volume = 0.8f }, Projectile.Center);
    }

    Player player = Main.player[Projectile.owner];

    // ← ИЗМЕНЕНО: проверяем, что игрок держит книгу И держит кнопку
    if (player.HeldItem.type != ModContent.ItemType<SightyTome>() || !player.controlUseItem)
    {
        Projectile.Kill();
        return;
    }

    // Обновляем время жизни (чтобы не исчезал)
    Projectile.timeLeft = 2;

    Projectile.localAI[1]++;

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

        // Когда портал полностью открылся
        if (Projectile.localAI[1] == 42)
        {
            // Вспышка света
            for (int i = 0; i < 25; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);

                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.BlueCrystalShard,
                    velocity
                );

                dust.noGravity = true;
                dust.scale = 1.5f;
            }

            float warpPulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 5f) * 0.05f;
        Projectile.scale = 1f + warpPulse;

    // Яркий световой импульс
    SoundEngine.PlaySound(SoundID.Item92 with { Volume = 1f }, Projectile.Center);
}

            // Позиция немного перед игроком
            Vector2 direction = Main.MouseWorld - player.Center;
            direction.Normalize();

            Projectile.Center = player.Center + direction * 80f;

            // Поворот портала
            Projectile.rotation = direction.ToRotation();
            Projectile.rotation += 0.02f;

            // Задержка 0.7 секунды (42 тика)
        if (Projectile.localAI[1] < 42)
        {
            return; // пока просто висим
        }

                // Искажение воздуха (вращающиеся энергетические частицы)
        if (Projectile.localAI[1] >= 42)
        {
            if (Main.rand.NextBool(2))
            {
                Vector2 circle = Main.rand.NextVector2CircularEdge(30f, 30f);

                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + circle,
                    DustID.Vortex,
                    circle.RotatedBy(MathHelper.PiOver2) * 0.05f
                );

                dust.noGravity = true;
                dust.scale = 1.3f;
            }
        }

                if (Main.rand.NextBool(4))
        {
            Dust dust = Dust.NewDustPerfect(
                Projectile.Center,
                DustID.BlueCrystalShard,
                Main.rand.NextVector2Circular(2f, 2f)
            );

            dust.noGravity = true;
            dust.scale = 1f;
        }

                if (Main.rand.NextBool(3))
        {
            Vector2 offset = Main.rand.NextVector2Circular(20f, 20f);

            Dust dust = Dust.NewDustPerfect(
                Projectile.Center + offset,
                DustID.PurpleTorch,
                Vector2.Zero
            );

            dust.noGravity = true;
            dust.scale = 1.2f;
            dust.velocity = offset * -0.05f;
        }

            // Анимация
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            // Стрельба
            if (Main.myPlayer == Projectile.owner)
            {
                if (Projectile.localAI[0]++ >= 6)
                {
                    Projectile.localAI[0] = 0;

                    Vector2 shootVel = direction * 40f;

                   Vector2 shootPos = Projectile.Center + direction * 20f; // 20f — расстояние вперёд

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