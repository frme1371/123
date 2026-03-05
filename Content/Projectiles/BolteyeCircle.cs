using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;
using myfirstmod.Content.Items;

namespace myfirstmod.Content.Projectiles
{
    public class BolteyeCircle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Статичный спрайт, без анимации
        }

        public override void SetDefaults()
        {
            Projectile.alpha = 255; // старт невидим

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

            // Проверяем, что игрок держит книгу И держит ПКМ
            if (player.HeldItem.type != ModContent.ItemType<SightyTome>() || player.altFunctionUse != 2)
            {
                Projectile.Kill();
                return;
            }

            // Обновляем время жизни (чтобы не исчезал)
            Projectile.timeLeft = 2;

            Projectile.localAI[1]++;

            // Спавним 3 круга только один раз в начале
            if (Projectile.localAI[1] == 1)
            {
                Vector2 direction = Main.MouseWorld - player.Center;
                direction.Normalize();

                Vector2 spawnPos = player.Center + direction * 80f;
                float rotation = direction.ToRotation();

                // Спавним маленький круг (он будет стрелять)
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<BolteyeCircle_Small>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );

                // Спавним средний круг (визуал)
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<BolteyeCircle_Medium>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );

                // Спавним большой круг (визуал)
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<BolteyeCircle_Large>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );

                SoundEngine.PlaySound(SoundID.Item122 with { Volume = 0.8f }, spawnPos);
            }

            // Обновляем позицию и поворот всех кругов
            Vector2 currentDirection = Main.MouseWorld - player.Center;
            currentDirection.Normalize();
            Vector2 centerPos = player.Center + currentDirection * 80f;
            float centerRotation = currentDirection.ToRotation();

            // Находим и обновляем все 3 круга
            UpdateAllCircles(centerPos, centerRotation);

            // Fade in анимация
            float fadeDuration = 42f;

            if (Projectile.localAI[1] <= fadeDuration)
            {
                float progress = Projectile.localAI[1] / fadeDuration;
                Projectile.alpha = (int)(255f * (1f - progress));
            }
            else
            {
                Projectile.alpha = 0;
            }
        }

        private void UpdateAllCircles(Vector2 centerPos, float rotation)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (!proj.active || proj.owner != Projectile.owner)
                    continue;

                if (proj.type == ModContent.ProjectileType<BolteyeCircle_Small>() ||
                    proj.type == ModContent.ProjectileType<BolteyeCircle_Medium>() ||
                    proj.type == ModContent.ProjectileType<BolteyeCircle_Large>())
                {
                    proj.Center = centerPos;
                    proj.rotation = rotation;
                }
            }
        }
    }
}