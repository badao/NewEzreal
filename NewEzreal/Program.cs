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
    class Program
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }

        public static Orbwalking.Orbwalker Orbwalker;

        public static Spell Q, W, E, R;

        public static Menu Menu;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Ezreal")
                return;

            Q = new Spell(SpellSlot.Q,1150);
            W = new Spell(SpellSlot.W,1000);
            E = new Spell(SpellSlot.E,475);
            R = new Spell(SpellSlot.R);
            Q.SetSkillshot(250,60,2000,true,SkillshotType.SkillshotLine);
            W.SetSkillshot(250, 80, 1600, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(1000,160,2000,false,SkillshotType.SkillshotLine);

            Menu = new Menu(Player.ChampionName, Player.ChampionName, true);
            Menu orbwalkerMenu = new Menu("Orbwalker", "Orbwalker");
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            Menu.AddSubMenu(orbwalkerMenu);
            Menu ts = Menu.AddSubMenu(new Menu("Target Selector", "Target Selector")); ;
            TargetSelector.AddToMenu(ts);
            Menu spellMenu = Menu.AddSubMenu(new Menu("Spells", "Spells"));

            Menu Harass = spellMenu.AddSubMenu(new Menu("Harass", "Harass"));

            Menu Combo = spellMenu.AddSubMenu(new Menu("Combo", "Combo"));

            Menu LaneClear = spellMenu.AddSubMenu(new Menu("LaneClear", "LaneClear"));

            Menu JungClear = spellMenu.AddSubMenu(new Menu("JungClear", "JungClear"));

            Menu Auto = spellMenu.AddSubMenu(new Menu("Auto","Auto"));

            Menu KillSteal = spellMenu.AddSubMenu(new Menu("KillSteal", "KillSteal"));

            Menu Draw = spellMenu.AddSubMenu(new Menu("Draw", "Draw"));

            Combo.AddItem(new MenuItem("Use Q Combo", "Use Q Combo").SetValue(true));
            Combo.AddItem(new MenuItem("Use W Combo", "Use W Combo").SetValue(true));
            Combo.AddItem(new MenuItem("Use R Combo", "Use R Combo").SetValue(true));
            Combo.AddItem(new MenuItem("Use R If Hit X Enemy", "Use R If Hit X Enemy").SetValue(new Slider(1, 1, 3)));
            Combo.AddItem(new MenuItem("Use Muramana", "Use Muramana").SetValue(true));
            Combo.AddItem(new MenuItem("Use Muramana if mana >", "Use Muramana if mana >").SetValue(new Slider(40, 0, 100)));
            Harass.AddItem(new MenuItem("Use Q Harass", "Use Q Harass").SetValue(true));
            Harass.AddItem(new MenuItem("Use W Harass", "Use W Harass").SetValue(true));
            Harass.AddItem(new MenuItem("minimum Mana HR", "minimum Mana HR").SetValue(new Slider(40, 0, 100)));
            LaneClear.AddItem(new MenuItem("Use Q LaneClear", "Use Q LaneClear").SetValue(true));
            LaneClear.AddItem(new MenuItem("minimum Mana LC", "minimum Mana LC").SetValue(new Slider(40, 0, 100)));
            JungClear.AddItem(new MenuItem("Use Q JungClear", "Use Q LaneClear").SetValue(true));
            JungClear.AddItem(new MenuItem("minimum Mana JC", "minimum Mana JC").SetValue(new Slider(40, 0, 100)));
            Auto.AddItem(new MenuItem("AutoQ", "AutoQ").SetValue(new KeyBind("H".ToCharArray()[0],KeyBindType.Toggle,false)));
            Auto.AddItem(new MenuItem("Keep Mana for E", "Keep Mana for E").SetValue(true));
            KillSteal.AddItem(new MenuItem("Use Q KillSteal", "Use Q KillSteal").SetValue(true));
            KillSteal.AddItem(new MenuItem("Use W KillSteal", "Use W KillSteal").SetValue(true));
            KillSteal.AddItem(new MenuItem("Use R KillSteal", "Use R KillSteal").SetValue(true));
            KillSteal.AddItem(new MenuItem("R Range KillSteal", "R Range KillSteal").SetValue(new Slider(1500, 800, 2500)));
            Draw.AddItem(new MenuItem("Draw Q", "Draw Q").SetValue(true));
            Draw.AddItem(new MenuItem("Draw W", "Draw W").SetValue(true));
            Draw.AddItem(new MenuItem("Draw E", "Draw E").SetValue(true));

            Menu.AddToMainMenu();
            Drawing.OnDraw += OnDraw;
            Orbwalking.OnAttack += OnAttack;
            Game.OnUpdate += Game_OnGameUpdate;
            Game.PrintChat("This is HuynMi's Ezreal!");
        }
        public static void OnAttack(AttackableUnit unit, AttackableUnit target)
        {

            if (unit.IsMe && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (ItemData.Youmuus_Ghostblade.GetItem().IsReady())
                    ItemData.Youmuus_Ghostblade.GetItem().Cast();
                if (ItemData.Muramana.GetItem().IsReady() && Menu.Item("Use Muramana").GetValue<bool>() && !Player.HasBuff("Muramana") && Player.Mana / Player.MaxMana * 100 >= Menu.Item("Use Muramana if mana >").GetValue<Slider>().Value)
                    ItemData.Muramana.GetItem().Cast();
                if (ItemData.Muramana.GetItem().IsReady() && Menu.Item("Use Muramana").GetValue<bool>() && Player.HasBuff("Muramana") && Player.Mana / Player.MaxMana * 100 < Menu.Item("Use Muramana if mana >").GetValue<Slider>().Value)
                    ItemData.Muramana.GetItem().Cast();
            }
        }
        private static void OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;
            if (Menu.Item("Draw Q").GetValue<bool>())
            {
                if (Q.IsReady())
                {
                    Render.Circle.DrawCircle(Player.Position, Q.Range, Color.Green);
                }
                else
                {
                    Render.Circle.DrawCircle(Player.Position, Q.Range, Color.Black);
                }
            }
            if (Menu.Item("Draw W").GetValue<bool>())
            {
                if (W.IsReady())
                {
                    Render.Circle.DrawCircle(Player.Position, W.Range, Color.Yellow);
                }
                else
                {
                    Render.Circle.DrawCircle(Player.Position, W.Range, Color.Black);
                }
            }
            if (Menu.Item("Draw E").GetValue<bool>())
            {
                if (E.IsReady())
                {
                    Render.Circle.DrawCircle(Player.Position, E.Range, Color.BlueViolet);
                }
                else
                {
                    Render.Circle.DrawCircle(Player.Position, E.Range, Color.Black);
                }
            }
        }
        public static void Game_OnGameUpdate(EventArgs args)
        {
            Combo.killsteal();
            Combo.auto();
            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                Combo.combo();
            }
            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                Combo.harass();

            }
            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                Combo.laneclear();
                Combo.jungclear();
            }
            //if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit)
            //{
            //    Combo.killsteal();
            //    Combo.auto();
            //}
        }
        public static double Dame(Obj_AI_Base target,SpellSlot x)
        {
            if (target != null) { return Player.GetSpellDamage(target, x); } else return 0;
        }

        public static void checkbuff()
        {
            String temp = "";
            foreach (var buff in Player.Buffs)
            {
                temp += (buff.Name + "(" + buff.Count + ")" + ", ");
            }
            Game.PrintChat(temp);
        }
    }
}
