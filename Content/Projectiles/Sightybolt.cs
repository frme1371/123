using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace myfirstmod.Content.Projectiles

{
    public class SightyBolt : ModProjectile
    {
        public override void SetStaticDefaults()
{
    
    Main.projFrames[Projectile.type] = 16; // количество кадров

    ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12; 
    ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
}


public override void PostDraw(Color lightColor)
{
    Texture2D glowTexture = ModContent.Request<Texture2D>(
        Texture + "_Glow").Value;

    Rectangle frame = glowTexture.Frame(
        1,
        Main.projFrames[Projectile.type],
        0,
        Projectile.frame);

    Vector2 drawPosition = Projectile.Center - Main.screenPosition;

    float pulse = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.5f + 1f);

    Main.EntitySpriteDraw(
        glowTexture,
        drawPosition,
        frame,
        Color.White * pulse, // делает свечение независимым от света
        Projectile.rotation,
        frame.Size() / 2,
        Projectile.scale,
        SpriteEffects.None,
        0
    );
}

public override bool PreDraw(ref Color lightColor)
{
    Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

    Rectangle frame = texture.Frame(
        1,
        Main.projFrames[Projectile.type],
        0,
        Projectile.frame
    );

    Vector2 origin = frame.Size() / 2f;

    for (int i = 0; i < Projectile.oldPos.Length; i++)
    {
        Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

        float alpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;

        Color color = new Color(120, 50, 255) * alpha * 0.6f;

        Main.EntitySpriteDraw(
            texture,
            drawPos,
            frame, // ← ВСЕГДА используем frame
            color,
            Projectile.rotation,
            origin,
            Projectile.scale,
            SpriteEffects.None,
            0
        );
    }

    return true;
}
    
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 1; // поведение как у пули
            AIType = ProjectileID.Bullet;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.penetrate = 7;
            Projectile.timeLeft = 600;

            Projectile.light = 0.5f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }
        public override void AI()
{
    Projectile.frameCounter++;

    if (Projectile.frameCounter >= 2) // скорость анимации
    {
        Projectile.frameCounter = 0;
        Projectile.frame++;

        if (Projectile.frame >= Main.projFrames[Projectile.type])
        {
            Projectile.frame = 0;
        }
    }
}
    }
}