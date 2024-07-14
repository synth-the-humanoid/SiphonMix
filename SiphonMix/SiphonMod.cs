using KHMI;
using KHMI.Types;

namespace SiphonMix
{
    public class SiphonMod : KHMod
    {
        public SiphonMod(ModInterface modInterface) : base(modInterface) { }

        private void setMaxHP(Entity target, int newHP)
        {
            StatPage sp = target.StatPage;
            if(sp != null)
            {
                sp.MaxHP = newHP;
                sp.CurrentHP = Math.Min(sp.CurrentHP, sp.MaxHP);
                PartyStatPage psp = sp.PartyStatPage;
                if(psp != null)
                {
                    psp.MaxHP = (byte)sp.MaxHP;
                }
            }
        }
        public override void onDamage(Entity target)
        {
            if(target.IsPartyMember)
            {
                int newHP = (target.StatPage.MaxHP + target.StatPage.CurrentHP) / 2;
                setMaxHP(target, newHP);
            }
            else
            {
                Entity player = Entity.getPlayer(modInterface.dataInterface);
                if(player != null)
                {
                    setMaxHP(player, player.StatPage.MaxHP + 1);
                }
            }
        }

        public override void onEntityDeath(Entity deceased)
        {
            Entity player = Entity.getPlayer(modInterface.dataInterface);
            if (deceased.IsPartyMember)
            {
                if(!deceased.IsPlayer)
                {
                    
                    setMaxHP(player, player.StatPage.MaxHP * 2 / 3);
                }
            }
            else
            {
                player.StatPage.CurrentHP = Math.Min(player.StatPage.CurrentHP + 1, player.StatPage.MaxHP);

            }
        }
    }
}
