using System;
using System.Collections.Generic;
using System.Text;

namespace ConsolePlatformer
{
    class MachineGun : IWeapon
    {
        public WeaponTypes Type { get; }
        public Rarities Rarity { get; }
        public int Damage { get; }
        public int FireRate { get; }
        public int MagazineSize { get; }
        public int BulletsInMagazine { get; private set; }
        public bool Equipped { get; private set; }
        private Player player;
        private Background background;

        public MachineGun(WeaponTypes type, Rarities rarity, Player player, Background background)
        {
            Type = type;
            Rarity = rarity;
            Damage = rarity == Rarities.COMMON ? 5 : rarity == Rarities.RARE ? 10 : 15;
            FireRate = rarity == Rarities.COMMON ? 15 : rarity == Rarities.RARE ? 10 : 5;
            this.player = player;
            this.background = background;
            MagazineSize = 15;
            BulletsInMagazine = MagazineSize;
        }
        public Projectile Fire()
        {
            if (BulletsInMagazine > 0)
            {
                BulletsInMagazine--;
                Projectile projectile = new Projectile(player.Position + 1, player.Bottom - 1, player.Direction, 5, Damage, background);
                return projectile;
            }
            else
            {
                Reload();
                BulletsInMagazine--;
                Projectile projectile = new Projectile(player.Position + 1, player.Bottom - 1, player.Direction, 5, Damage, background);
                return projectile;
            }
        }

        public void Reload()
        {
            //Display reloading...
            //wait for reload?
            BulletsInMagazine = MagazineSize;
        }

        public void Equip()
        {
            Equipped = true;
        }
    }
}
