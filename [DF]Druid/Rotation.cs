using System;
using System.Threading;
using wShadow.Templates;
using wShadow.Warcraft.Classes;
using wShadow.Warcraft.Defines;
using wShadow.Warcraft.Managers;
using wShadow.Warcraft.Structures.Wow_Player;
using wShadow.Warcraft.Defines.Wow_Player;
using wShadow.Warcraft.Defines.Wow_Spell;



public class DFDruid : Rotation
{
	
    private int debugInterval = 5; // Set the debug interval in seconds
    private DateTime lastDebugTime = DateTime.MinValue;

	
    public override void Initialize()
    {
        // Can set min/max levels required for this rotation.
        
		 lastDebugTime = DateTime.Now;
        LogPlayerStats();
        // Use this method to set your tick speeds.
        // The simplest calculation for optimal ticks (to avoid key spam and false attempts)

		// Assuming wShadow is an instance of some class containing UnitRatings property
        SlowTick = 600;
        FastTick = 200;

        // You can also use this method to add to various action lists.

        // This will add an action to the internal passive tick.
        // bool: needTarget -> If true action will not fire if player does not have a target
        // Func<bool>: function -> Action to attempt, must return true or false.
        PassiveActions.Add((true, () => false));

        // This will add an action to the internal combat tick.
        // bool: needTarget -> If true action will not fire if player does not have a target
        // Func<bool>: function -> Action to attempt, must return true or false.
        CombatActions.Add((true, () => false));
		
		
		
    }
	public override bool PassivePulse()
	{
			

		 var me = Api.Player;
		var healthPercentage = me.HealthPercent;
		var mana = me.Mana;

			if (me.IsDead() || me.IsGhost() || me.IsCasting()|| me.IsMoving() || me.IsMounted() ) return false;
        if (me.HasAura("Drink") || me.HasAura("Food")) return false;
		if ((DateTime.Now - lastDebugTime).TotalSeconds >= debugInterval)
        {
            LogPlayerStats();
            lastDebugTime = DateTime.Now; // Update lastDebugTime
        }

if (Api.Spellbook.CanCast("Mark of the Wild") && !me.HasAura("Mark of the Wild")  )
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Casting Mark of the Wild");
    Console.ResetColor();
    if (Api.Spellbook.Cast("Mark of the Wild"))
   
        return true;
    }



			

			 var target = Api.Target;
if (!me.HasPermanent("Cat Form") && Api.Spellbook.CanCast("Cat Form") )	
		{	
		if (Api.Spellbook.CanCast("Cat Form") && !me.HasPermanent("Cat Form") )
				{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Casting Cat Form");
			Console.ResetColor();
		if (Api.Spellbook.Cast("Cat Form"))
				{
					return true;
				}
				}
				
			}
							


 return base.PassivePulse();
}				
		
	public override bool CombatPulse()
    {
				

        var me = Api.Player;
		var healthPercentage = me.HealthPercent;
		var mana = me.Mana;
		 var target = Api.Target;
		var targethealth = target.HealthPercent;
		var manapercent = me.ManaPercent;
		var energy = me.Energy;
		var points = me.ComboPoints;
		
		if (Api.Spellbook.CanCast("Rejuvenation") &&!me.HasAura("Rejuvenation") && healthPercentage <= 85 && mana > 15)
        {
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Casting Rejuvenation");
			Console.ResetColor();
			if (Api.Spellbook.Cast("Rejuvenation"))
			{
				return true;
			}
		}
		if (Api.Spellbook.CanCast("Regrowth") &&!me.HasAura("Regrowth") && healthPercentage <= 55 && mana > 15)
        {
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Casting Regrowth");
			Console.ResetColor();
			if (Api.Spellbook.Cast("Regrowth"))
			{
				return true;
			}
		}
		
		if (Api.Spellbook.CanCast("Healing Touch") && healthPercentage <= 45 && mana > 25)
        {
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Casting Healing Touch");
			Console.ResetColor();
			if (Api.Spellbook.Cast("Healing Touch"))
			{
				return true;
			}
       }
	if (Api.Spellbook.CanCast("Wrath") && !me.HasPermanent("Cat Form"))
	   {
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Casting Wrath");
			Console.ResetColor();
			if (Api.Spellbook.Cast("Wrath"))
			{
				return true;
			}		   
	   }
		if (Api.Spellbook.CanCast("Cat Form") && !me.HasPermanent("Cat Form") )
				{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Casting Cat Form");
			Console.ResetColor();
		if (Api.Spellbook.Cast("Cat Form"))
				{
					return true;
				}
				}
				
			
			
		
		if (Api.Spellbook.CanCast("Tiger's Fury") && me.HasPermanent("Cat Form") && !Api.Spellbook.OnCooldown("Tiger's Fury"))
				{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Casting Tiger's Fury");
			Console.ResetColor();
		if (Api.Spellbook.Cast("Tiger's Fury"))
				{
					return true;
				}
				}
				
		if (Api.Spellbook.CanCast("Rip") && !target.HasAura("Rip") && me.HasAura("Tiger's Fury") && points >= 3 && me.HasPermanent("Cat Form") )
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Casting Rip with {points} Points and {energy} Energy");
			Console.ResetColor();

		if (Api.Spellbook.Cast("Rip"))
				return true;
		}		
		if (Api.Spellbook.CanCast("Shred") &&  me.HasPermanent("Cat Form") && me.HasAura("Clearcasting"))
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Casting Shred with clearcasting");
			Console.ResetColor();

		if (Api.Spellbook.Cast("Shred"))
			return true;
		}	
		if (Api.HasMacro("Rake") && points < 3 && energy >= 35  && !target.HasAura("Rake") && me.HasPermanent("Cat Form"))
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Casting Rake");
			Console.ResetColor();

		if (Api.Spellbook.Cast("Rake"))
				return true;
		}
	if (Api.Spellbook.CanCast("Rip") && !target.HasAura("Rip") && target.HealthPercent >= 20 && energy > 30 && points == 3 && me.HasPermanent("Cat Form"))
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Casting Rip with {points} Points and {energy} Energy");
			Console.ResetColor();

		if (Api.Spellbook.Cast("Rip"))
				return true;
		}
		if (Api.Spellbook.CanCast("Swipe") && points < 3 && energy >= 45 && me.HasPermanent("Cat Form") && Api.UnfriendlyUnitsNearby(10, true) >= 2)
{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Casting Swipe for aoe");
			Console.ResetColor();

		if (Api.Spellbook.Cast("Swipe"))
			return true;
		}
		else if (Api.Spellbook.CanCast("Claw") && points < 3 && energy >= 45 && me.HasPermanent("Cat Form"))
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Casting Claw (Cat) with {energy} Energy");
			Console.ResetColor();

		if (Api.Spellbook.Cast("Claw"))
			return true;
		}
		if (Api.Spellbook.CanCast("Shred") && points < 3 && energy >= 45 && me.HasPermanent("Cat Form"))
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Casting Shred");
			Console.ResetColor();

		if (Api.Spellbook.Cast("Shred"))
			return true;
		}

		
		if (Api.Spellbook.CanCast("Ferocious Bite")  && energy > 30 && points >= 3 && me.HasPermanent("Cat Form"))
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Casting Ferocious Bite");
			Console.ResetColor();

		if (Api.Spellbook.Cast("Ferocious Bite"))
				return true;
		}
		if (!target.HasAura("Moonfire") && targethealth > 65 && manapercent > 5 && !target.HasAura("Moonfire")  && !me.HasPermanent("Cat Form"))
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Casting Moonfire");
			Console.ResetColor();
			if (Api.Spellbook.Cast("Moonfire"))
			{
				return true;
			}
		}

        if (Api.Spellbook.CanCast("Wrath") && !me.HasPermanent("Cat Form"))
	   {
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Casting Wrath");
			Console.ResetColor();
			if (Api.Spellbook.Cast("Wrath"))
			{
				return true;
			}		   
	   }

	
		return base.CombatPulse();
    }
	
	private void LogPlayerStats()
    {
        var me = Api.Player;

		var mana = me.Mana;
        var healthPercentage = me.HealthPercent;
		
if (Api.Spellbook.CanCast("Wrath") )

    {
       
        
       
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("CanCast Wrath");
            Console.ResetColor();
            
                  
        
    }
	if (Api.Spellbook.CanCast("Moonfire") )

    {
       
        
       
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("CanCast Moonfire");
            Console.ResetColor();
            
                  
        
    }
	
	// Check for "Regrowth" aura
if (me.HasAura("Regrowth"))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Have aura Regrowth");
    Console.ResetColor();
}

// Assuming there's a method HasPermanent for checking permanent auras
if (me.HasPermanent("Regrowth"))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Have permanent Regrowth");
    Console.ResetColor();
}

// Assuming there's a method HasPassive for checking passive auras
if (me.HasPassive("Regrowth"))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Have Passive Regrowth");
    Console.ResetColor();
}
if (me.HasAura("Cat Form"))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Have aura Cat Form");
    Console.ResetColor();
}

// Assuming there's a method HasPermanent for checking permanent auras
if (me.HasPermanent("Cat Form"))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Have permanent Cat Form");
    Console.ResetColor();
}

// Assuming there's a method HasPassive for checking passive auras
if (me.HasPassive("Cat Form"))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Have Passive Cat Form");
    Console.ResetColor();
}

if (me.HasAura("Travel Form"))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Have aura Travel Form");
    Console.ResetColor();
}

// Assuming there's a method HasPermanent for checking permanent auras
if (me.HasPermanent("Travel Form"))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Have permanent Travel Form");
    Console.ResetColor();
}

// Assuming there's a Tmethod HasPassive for checking passive auras
if (me.HasPassive("Travel Form"))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Have Passive Travel Form");
    Console.ResetColor();
}
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{mana} Mana available");
        Console.WriteLine($"{healthPercentage}%Health available");
		Console.ResetColor();

    }
}