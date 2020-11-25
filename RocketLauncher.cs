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
        public double ReloadSpeed { get; }
        public int BulletsInMagazine { get; private set; }
        public bool Equipped { get; private set; }
        private Player player;
        private Background background;

        public RocketLauncher(WeaponTypes type, Rarities rarity, int magSize, int damage, double reloadSpeed, Player player, Background background)
        {
            Type = type;
            Rarity = rarity;
            Damage = damage;
            MagazineSize = magSize;
            ReloadSpeed = reloadSpeed;
            FireRate = rarity == Rarities.COMMON ? 5 : 3;
            this.player = player;
            this.background = background;
            BulletsInMagazine = MagazineSize;
        }
        public List<Projectile> Fire()
        {
            List<Projectile> shots = new List<Projectile>();
            BulletsInMagazine--;
            Projectile projectile = new Projectile(ProjectileType.BULLET, player.Position + 1, player.Bottom - 1, player.Direction, 5, Damage, background);
            shots.Add(projectile);
            return shots;
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
