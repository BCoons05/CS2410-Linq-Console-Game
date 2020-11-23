using System;
using System.Collections.Generic;

namespace ConsolePlatformer
{
    class RocketLauncher : IWeapon
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

        public RocketLauncher(WeaponTypes type, Rarities rarity, Player player, Background background)
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
        public List<Projectile> Fire()
        {
            List<Projectile> shots = new List<Projectile>();
            if (BulletsInMagazine > 0)
            {
                BulletsInMagazine--;
                Projectile projectile = new Projectile(ProjectileType.BULLET, player.Position + 1, player.Bottom - 1, player.Direction, 5, Damage, background);
                shots.Add(projectile);
                return shots;
            }
            else
            {
                Reload();
                BulletsInMagazine--;
                Projectile projectile = new Projectile(ProjectileType.BULLET, player.Position + 1, player.Bottom - 1, player.Direction, 5, Damage, background);
                shots.Add(projectile);
                return shots;
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

        public override string ToString()
        {
            return $"{Rarity} {Type} Damage: {Damage} Mag size: {MagazineSize}";
        }
    }
}
