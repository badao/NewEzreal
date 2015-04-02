using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using SharpDX;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Common.Data;

namespace NewEzreal
{
    class Combo
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        public static void combo()
        {
            if(Program.Q.IsReady() && !Player.IsWindingUp)
            {
                var target = TargetSelector.GetTarget(Program.Q.Range, TargetSelector.DamageType.Physical);
                if (target != null && target.IsValidTarget() && !target.IsZombie && Program.Menu.Item("Use Q Combo").GetValue<bool>())
                {
                    var pred = Program.Q.GetPrediction(target); if (pred.Hitchance >= HitChance.Medium) { Program.Q.Cast(target); }
                    else { foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsEnemy)) { if (hero != null && hero.IsValidTarget() && !hero.IsZombie) Program.Q.Cast(hero); } }
                }
                
            }
            if (Program.W.IsReady() && !Player.IsWindingUp)
            {
                var target = TargetSelector.GetTarget(Program.W.Range, TargetSelector.DamageType.Magical);
                if (target != null && target.IsValidTarget() && !target.IsZombie && Program.Menu.Item("Use W Combo").GetValue<bool>())
                {
                    var pred = Program.W.GetPrediction(target); if (pred.Hitchance >= HitChance.Low) { Program.W.Cast(target); }
                    else { foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsEnemy)) { if (hero != null && hero.IsValidTarget() && !hero.IsZombie)Program.W.Cast(hero);  } }
                }
            }
            if (Program.R.IsReady() && !Player.IsWindingUp)
            {
                var target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Magical);
                if(target != null && target.IsValidTarget() && !target.IsZombie && Program.Menu.Item("Use R Combo").GetValue<bool>())
                { var x = Program.Menu.Item("Use R If Hit X Enemy").GetValue<Slider>().Value; Program.R.CastIfWillHit(target,x);}
            }
            if (ItemData.Bilgewater_Cutlass.GetItem().IsReady())
            {
                var target = TargetSelector.GetTarget(ItemData.Bilgewater_Cutlass.GetItem().Range, TargetSelector.DamageType.Physical);
                if (target != null && target.IsValidTarget() &&!target.IsZombie)
                    ItemData.Bilgewater_Cutlass.GetItem().Cast(target);
            }
            if (ItemData.Blade_of_the_Ruined_King.GetItem().IsReady())
            {
                var target = TargetSelector.GetTarget(ItemData.Blade_of_the_Ruined_King.GetItem().Range, TargetSelector.DamageType.Physical);
                if (target != null && target.IsValidTarget() && !target.IsZombie)
                    ItemData.Blade_of_the_Ruined_King.GetItem().Cast(target);
            }
        }
        public static void harass()
        {
            if (Program.Q.IsReady()&& !Player.IsWindingUp)
            {
                var target = TargetSelector.GetTarget(Program.Q.Range, TargetSelector.DamageType.Physical);
                if (target != null && target.IsValidTarget() && !target.IsZombie && Program.Menu.Item("Use Q Harass").GetValue<bool>() && Player.Mana / Player.MaxMana * 100 >= Program.Menu.Item("minimum Mana HR").GetValue<Slider>().Value)
                {
                    var pred = Program.Q.GetPrediction(target); if (pred.Hitchance >= HitChance.Medium) { Program.Q.Cast(target); }
                    else { foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsEnemy)) { if (hero != null && hero.IsValidTarget() && !hero.IsZombie)Program.Q.Cast(hero);  } }
                }
            }
            if (Program.W.IsReady() && !Player.IsWindingUp)
            {
                var target = TargetSelector.GetTarget(Program.W.Range, TargetSelector.DamageType.Physical);
                if (target != null && target.IsValidTarget() && !target.IsZombie && Program.Menu.Item("Use W Harass").GetValue<bool>() && Player.Mana / Player.MaxMana * 100 >= Program.Menu.Item("minimum Mana HR").GetValue<Slider>().Value)
                {
                    var pred = Program.W.GetPrediction(target); if (pred.Hitchance >= HitChance.Medium) { Program.W.Cast(target); }
                    else { foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsEnemy)) { if (hero != null && hero.IsValidTarget() && !hero.IsZombie)Program.W.Cast(hero); } }
                }
            }
        }
        public static void laneclear()
        {
            if (Program.Q.IsReady() && !Player.IsWindingUp)
            {
                var target = MinionManager.GetMinions(Player.Position, Program.Q.Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.Health).FirstOrDefault();
                if (Program.Menu.Item("Use Q LaneClear").GetValue<bool>() && Player.Mana / Player.MaxMana * 100 > Program.Menu.Item("minimum Mana LC").GetValue<Slider>().Value && target != null && target.IsValidTarget() && !target.IsZombie && !Player.IsWindingUp)
                { Program.Q.Cast(target); }
            }
        }
        public static void jungclear()
        {
            if (Program.Q.IsReady() && !Player.IsWindingUp)
            {
                var target = MinionManager.GetMinions(Player.Position, 600, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();
                if (Program.Menu.Item("Use Q JungClear").GetValue<bool>() && Player.Mana / Player.MaxMana * 100 > Program.Menu.Item("minimum Mana JC").GetValue<Slider>().Value && target != null && target.IsValidTarget() && !target.IsZombie && !Player.IsWindingUp)
                { Program.Q.Cast(target); }
            }
        }
        public static void killsteal()
        {
            if (Program.Q.IsReady() && Program.Menu.Item("Use Q KillSteal").GetValue<bool>() && !Player.IsWindingUp)
            {
               foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsEnemy && hero.IsValidTarget(Program.Q.Range)))
               {
                   var dmg = Program.Dame(hero, SpellSlot.Q);
                   if (hero != null && hero.IsValidTarget() && !hero.IsZombie && dmg > hero.Health) { Program.Q.Cast(hero); }
               }
            }
            if (Program.W.IsReady() && Program.Menu.Item("Use W KillSteal").GetValue<bool>() && !Player.IsWindingUp)
            {
                foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsEnemy && hero.IsValidTarget(Program.W.Range)))
                {
                    var dmg = Program.Dame(hero, SpellSlot.W);
                    if (hero != null && hero.IsValidTarget() && !hero.IsZombie && dmg > hero.Health) { Program.W.Cast(hero); }
                }
            }
            if (Program.R.IsReady() && Program.Menu.Item("Use R KillSteal").GetValue<bool>() && !Player.IsWindingUp)
            {
                var x = Program.Menu.Item("R Range KillSteal").GetValue<Slider>().Value;
                foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsEnemy && hero.IsValidTarget(x)))
                {
                    var dmg = Program.Dame(hero, SpellSlot.R)*70/100;
                    if (hero != null && hero.IsValidTarget() && !hero.IsZombie && dmg > hero.Health) { Program.R.Cast(hero); }
                }
            }
        }
        public static void auto()
        {
            if (Program.Q.IsReady() && Program.Menu.Item("AutoQ").GetValue<KeyBind>().Active && !Player.IsWindingUp)
            {
                bool x = Program.Menu.Item("Keep Mana for E").GetValue<bool>();
                if (x == true && Player.Mana < Program.Q.Instance.ManaCost + Program.E.Instance.ManaCost)
                    return;
                else
                {
                    var target = TargetSelector.GetTarget(Program.Q.Range, TargetSelector.DamageType.Physical);
                    if (target != null && target.IsValidTarget() && !target.IsZombie)
                    {
                        var pred = Program.Q.GetPrediction(target); if (pred.Hitchance >= HitChance.Medium) { Program.Q.Cast(target); }
                        else { foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsEnemy)) { if (hero != null && hero.IsValidTarget() && !hero.IsZombie)Program.Q.Cast(hero); } }
                    }
                }
            }
        }
    }
}
