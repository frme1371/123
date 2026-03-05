using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace myfirstmod.Content.Items
{
    public class SightyTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 10));
        }

        

        public override bool CanUseItem(Player player)
{
    return player.ownedProjectileCounts[
        ModContent.ProjectileType<Content.Projectiles.BolteyePortal>()
    ] <= 0;
}

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Magic;

            Item.width = 57;
            Item.height = 58;

            Item.useTime = 1;
            Item.useAnimation = 10000;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.noMelee = true;
            Item.noUseGraphic = false;

            Item.knockBack = 4;
            Item.mana = 0;

            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;

            Item.UseSound = SoundID.Item20;
            Item.autoReuse = false;

            Item.noUseGraphic = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true; // держим кнопку
            Item.shoot = ModContent.ProjectileType<Content.Projectiles.BolteyePortal>();
            Item.shootSpeed = 0f; // ВАЖНО! портал сам двигается

            Item.consumable = false;
            Item.useTurn = false;
        }

        public override bool? UseItem(Player player)
{
    // Если игрок отпустил кнопку — сбросить прогресс использования
    if (!player.controlUseItem)
    {
        player.itemTime = 0;
        player.itemAnimation = 0;
    }
    
    return true;
}

        public override bool Shoot(
    Player player,
    Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
    Vector2 position,
    Vector2 velocity,
    int type,
    int damage,
    float knockback)
{
    if (player.ownedProjectileCounts[
        ModContent.ProjectileType<Content.Projectiles.BolteyePortal>()
    ] <= 0)
    {
        Projectile.NewProjectile(
            source,
            position,
            Vector2.Zero,
            ModContent.ProjectileType<Content.Projectiles.BolteyePortal>(),
            damage,
            knockback,
            player.whoAmI
        );
    }

    return false;
}



        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}