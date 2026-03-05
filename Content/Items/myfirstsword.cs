using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace myfirstmod.Content.Items
{
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class myfirstsword : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.myfirstmod.hjson' file.
		public override void SetDefaults()
		{
			Item.damage = 127;
			Item.DamageType = DamageClass.Melee;

			Item.width = 60;
			Item.height = 60;

			Item.useTime = 7;
			Item.useAnimation = 17;
			Item.useStyle = ItemUseStyleID.Swing;

			Item.knockBack = 1;
			Item.value = Item.buyPrice(silver: 10);
			Item.rare = ItemRarityID.Pink;

			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			
			Item.useTurn = true;
			Item.noUseGraphic = false;
			Item.noMelee = false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override void HoldItem(Player player)
		{
    		Lighting.AddLight(player.Center, 0.7f, 0.2f, 1f); // Фиолетовый цвет
		}

		public override void PostUpdate()
		{
    		Lighting.AddLight(Item.Center, 0.7f, 0.2f, 1f);
		}
	}
}
