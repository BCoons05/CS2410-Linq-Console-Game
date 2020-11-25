using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ConsolePlatformer
{
    class Shotgun : IWeapon
    {
        public WeaponTypes Type { get; }
        public Rarities Rarity { get; }
        public int Damage { get; }
        public int FireRate { get; }
        public int MagazineSize { get; }
        public double ReloadSpeed { get; }
        public int BulletsInMagazine { get; private set; }
        public bool Equipped { get; private set; }
        public bool Reloading { get; set; }
        private Player player;
        private Background background;

        public Shotgun(WeaponTypes type, Rarities rarity, int magSize, int damage, double reloadSpeed, Player player, Background background)
        {
            Type = type;
            Rarity = rarity;
            Damage = damage;
            MagazineSize = magSize;
            ReloadSpeed = reloadSpeed;
            FireRate = rarity == Rarities.COMMON ? 2 : 1;
            this.player = player;
            this.background = background;
            BulletsInMagazine = MagazineSize;
        }
        public List<Projectile> Fire()
        {
            List<Projectile> shots = new List<Projectile>();

            //TODO refactor the projectile creation and load
            BulletsInMagazine--;
            switch (player.Direction)
            {
                case Directions.DOWN:
                    Projectile projectile = new Projectile(ProjectileType.PELLET, player.Position + 1, player.Bottom + 1, player.Direction, FireRate, Damage, background);
                    Projectile projectile2 = new Projectile(ProjectileType.PELLET, player.Position, player.Bottom + 1, player.Direction, FireRate, Damage, background);
                    Projectile projectile3 = new Projectile(ProjectileType.PELLET, player.Position - 1, player.Bottom + 1, player.Direction, FireRate, Damage, background);
                    shots.Add(projectile);
                    shots.Add(projectile2);
                    shots.Add(projectile3);
                    break;
                case Directions.RIGHT:
                    projectile = new Projectile(ProjectileType.PELLET, player.Position + 1, player.Bottom - 1, player.Direction, FireRate, Damage, background);
                    projectile2 = new Projectile(ProjectileType.PELLET, player.Position + 1, player.Bottom, player.Direction, FireRate, Damage, background);
                    projectile3 = new Projectile(ProjectileType.PELLET, player.Position + 1, player.Bottom - 2, player.Direction, FireRate, Damage, background);
                    shots.Add(projectile);
                    shots.Add(projectile2);
                    shots.Add(projectile3);
                    break;
                case Directions.LEFT:
                    projectile = new Projectile(ProjectileType.PELLET, player.Position - 1, player.Bottom - 1, player.Direction, FireRate, Damage, background);
                    projectile2 = new Projectile(ProjectileType.PELLET, player.Position - 1, player.Bottom, player.Direction, FireRate, Damage, background);
                    projectile3 = new Projectile(ProjectileType.PELLET, player.Position - 1, player.Bottom - 2, player.Direction, FireRate, Damage, background);
                    shots.Add(projectile);
                    shots.Add(projectile2);
                    shots.Add(projectile3);
                    break;
                case Directions.UP:
                    projectile = new Projectile(ProjectileType.PELLET, player.Position + 1, player.Bottom - 2, player.Direction, FireRate, Damage, background);
                    projectile2 = new Projectile(ProjectileType.PELLET, player.Position, player.Bottom -2, player.Direction, FireRate, Damage, background);
                    projectile3 = new Projectile(ProjectileType.PELLET, player.Position - 1, player.Bottom - 2, player.Direction, FireRate, Damage, background);
                    shots.Add(projectile);
                    shots.Add(projectile2);
                    shots.Add(projectile3);
                    break;
            }
                
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
