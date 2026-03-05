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
            // Проверяем текущий тип атаки
            bool isAltAttack = player.altFunctionUse == 2; // ПКМ = altFunctionUse == 2

            if (isAltAttack)
            {
                // Для альт. атаки (круги)
                return player.ownedProjectileCounts[
                    ModContent.ProjectileType<Content.Projectiles.BolteyeCircle>()
                ] <= 0;
            }
            else
            {
                // Для основной атаки (портал)
                return player.ownedProjectileCounts[
                    ModContent.ProjectileType<Content.Projectiles.BolteyePortal>()
                ] <= 0;
            }
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

            Item.channel = true; // держим кнопку (ЛКМ и ПКМ)
            Item.shoot = ModContent.ProjectileType<Content.Projectiles.BolteyePortal>();
            Item.shootSpeed = 0f;

            Item.consumable = false;
            Item.useTurn = false;
        }

        public override bool? UseItem(Player player)
        {
            // Если игрок отпустил кнопку — сбросить прогресс использования
            if (!player.controlUseItem && player.altFunctionUse != 2)
            {
                player.itemTime = 0;
                player.itemAnimation = 0;
            }
            
            if (!player.controlUseTile && player.altFunctionUse == 2)
            {
                player.itemTime = 0;
                player.itemAnimation = 0;
            }

            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true; // Разрешаем альт. использование (ПКМ)
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
            bool isAltAttack = player.altFunctionUse == 2;

            if (isAltAttack)
            {
                // Альт. атака - спавним круги
                if (player.ownedProjectileCounts[
                    ModContent.ProjectileType<Content.Projectiles.BolteyeCircle>()
                ] <= 0)
                {
                    Projectile.NewProjectile(
                        source,
                        position,
                        Vector2.Zero,
                        ModContent.ProjectileType<Content.Projectiles.BolteyeCircle>(),
                        damage,
                        knockback,
                        player.whoAmI
                    );
                }
            }
            else
            {
                // Основная атака - спавним портал
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