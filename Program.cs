using System;
using System.Threading;
// =====================
// === BASE CHARACTER ===
// =====================
public class Character
{
    public string name;
    public int health;
    public int attack;
    public int defense;

    // Flags / Temporary buffs
    public bool isDefending = false;
    public int tempDefenseBonus = 0;
    public bool skipTurn = false; // stun/dizzy flag
    protected static Random rand = new Random();

    public Character(string name, int health, int attack, int defense)
    {
        this.name = name;
        this.health = health;
        this.attack = attack;
        this.defense = defense;
    }

    protected void WriteColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public void PrintHPBar()
    {
        int barLength = health / 5;
        string bar = new string('█', barLength);
        Console.Write("[");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(bar);
        Console.ResetColor();
        Console.WriteLine($"] {health} HP");
    }

    public virtual void Attack(Character target, bool isPlayer = false)
    {
        // Dodge check
        if (target is Robinette r && r.TryDodge())
        {
            WriteColored($"🟩 {target.name} dodged the attack!", ConsoleColor.Green);
            return;
        }

        int baseDamage = attack - (target.defense / 2);
        int variance = rand.Next(-2, 3);
        int totalDamage = Math.Max(0, baseDamage + variance);

        // If target is defending → block + parry
        if (target.isDefending)
        {
            int reducedDamage = totalDamage / 2;
            double parryRatio = rand.Next(40, 66) / 100.0;
            int reflectedDamage = (int)(totalDamage * parryRatio);

            target.health -= reducedDamage;
            target.health = Math.Max(0, target.health);

            this.health -= reflectedDamage;
            this.health = Math.Max(0, this.health);

            WriteColored($"🟦 {target.name} BLOCKS! Damage reduced to {reducedDamage}, reflects {reflectedDamage} to {name}.", ConsoleColor.Blue);
            Console.WriteLine($"⚔️ {name} takes {reflectedDamage} damage from block! ({this.health} HP)");
            Console.WriteLine($"⚔️ {target.name} takes {reducedDamage} damage after block! ({target.health} HP)");
        }
        else
        {
            // Normal attack
            target.health -= totalDamage;
            target.health = Math.Max(0, target.health);
            Console.ForegroundColor = isPlayer ? ConsoleColor.Yellow : ConsoleColor.Red;
            Console.WriteLine($"⚔️ {name} hits {target.name} for {totalDamage} damage!");
            Console.ResetColor();
        }
    }

    public virtual void Defend(bool isPlayer = false)
    {
        if (!isDefending)
        {
            tempDefenseBonus = 3;
            defense += tempDefenseBonus;
            isDefending = true;
            WriteColored($"🛡️ {name} raises defense for 1 turn!", ConsoleColor.Cyan);
        }
        else
        {
            WriteColored($"⚠️ {name} is already defending!", ConsoleColor.DarkYellow);
        }
    }

    public bool IsAlive() => health > 0;
    public virtual bool TryDodge() => false; // base δεν έχει dodge
    
}

// =====================
// === SPECIAL CHARACTERS ===
// =====================
public class Calcium : Character
{
    public Calcium() : base("Calcium", 115, 17, 10) { }
    public override void Attack(Character target, bool isPlayer = false)
    {
        WriteColored($"{name} hurls spooky bones! 💀", ConsoleColor.White);
        Thread.Sleep(500);

        string[] frames = { "🦴 →", "     🦴 →", "          🦴 →", "                💥" };
        foreach (var f in frames)
        {
            Console.WriteLine(f);
            Thread.Sleep(200);
        }

        base.Attack(target, isPlayer);

        // Double hit logic
        if (rand.NextDouble() < 0.33 && target.IsAlive())
        {
            WriteColored($"🟦 DOUBLE HIT! {name}'s bones bounce again!", ConsoleColor.Blue);
            base.Attack(target, isPlayer);
        }
    }
}

// --- Robinette ---
public class Robinette : Character
{
    private int maxHealth;

    public Robinette() : base("Robinette", 95, 18, 9)
    {
        maxHealth = health;
    }

    // Προσάρμοσε το TryDodge ώστε να έχει extra dodge κάτω από 20% HP
    public override bool TryDodge()
    {
        double baseChance = 0.25;
        double bonus = (health < maxHealth * 0.20) ? 0.25 : 0.0;
        double finalChance = baseChance + bonus;

        bool dodged = rand.NextDouble() < finalChance;

        if (health < maxHealth * 0.20)
        {
            WriteColored($"💥 {name} is in Last Hope mode! (+25% dodge chance)", ConsoleColor.Cyan);
        }

        return dodged;
    }

    public override void Attack(Character target, bool isPlayer = false)
    {
        WriteColored($"{name} draws her bow... 🎯", ConsoleColor.Yellow);
        Thread.Sleep(400);

        string[] frames = { "🏹 →", "     ➤ →", "         ➤ →", "               💥" };
        foreach (var f in frames)
        {
            Console.WriteLine(f);
            Thread.Sleep(180);
        }

        WriteColored($"{name} fires a piercing arrow! 🏹🔥", ConsoleColor.Yellow);
        Thread.Sleep(400);

        base.Attack(target, isPlayer);
    }

}


public class CL4NK : Character
{
    public CL4NK() : base("CL4NK", 120, 17, 12) { }
    public override void Attack(Character target, bool isPlayer = false)
    {
        WriteColored($"{name} locks target... 🔫", ConsoleColor.Gray);
        Thread.Sleep(300);

        string[] frames = {"🔫 →", "    💥", "🔫 →", "    💥", "🔫 →", "    💥" };
        foreach (var f in frames)
        {
            Console.WriteLine(f);
            Thread.Sleep(150);
        }

        WriteColored($"{name} fires mechanical revolver rounds! 💥🤖", ConsoleColor.DarkGray);
        Thread.Sleep(400);

        // Critical logic
        if (rand.NextDouble() < 0.33)
        {
            int critDamage = (attack * 2) - (target.defense / 2);
            target.health -= critDamage;
            target.health = Math.Max(0, target.health);
            WriteColored($"🟥 CRITICAL HIT! {name} deals {critDamage} damage!", ConsoleColor.Red);
        }
        else
        {
            base.Attack(target, isPlayer);
        }
    }

}

public class Megachad : Character
{
    public Megachad() : base("Megachad", 130, 20, 9) { }
    public override void Attack(Character target, bool isPlayer = false)
    {
        WriteColored($"{name} flexes intensely... 💪🔥", ConsoleColor.Magenta);
        Thread.Sleep(500);

        string[] frames = { "💪", "💪💥", "💪💥💥", "💥💥💥" };
        foreach (var f in frames)
        {
            Console.WriteLine(f);
            Thread.Sleep(250);
        }

        WriteColored($"{name} unleashes raw CHAD ENERGY! ⚡", ConsoleColor.Magenta);
        Thread.Sleep(400);

        base.Attack(target, isPlayer);

        if (rand.NextDouble() < 0.25 && target.IsAlive())
        {
            target.skipTurn = true;
            WriteColored($"💫 {target.name} is overwhelmed and skips next turn!", ConsoleColor.Cyan);
        }
    }

}

public class SirOofie : Character
{
    public SirOofie() : base("Sir Oofie", 125, 16, 15) { }
    public override void Defend(bool isPlayer = false)
    {
        base.Defend(isPlayer);
        if (rand.NextDouble() < 0.30)
        {
            int bonus = (int)(defense * 0.5);
            defense += bonus;
            WriteColored($"🟦 {name}'s armor shines! +{bonus} temporary DEF!", ConsoleColor.Blue);
        }
    }
    public override void Attack(Character target, bool isPlayer = false)
    {
        // 💠 Armor Shine BEFORE attack — ισχύει για αυτόν τον γύρο
        if (rand.NextDouble() < 0.30)
        {
            int bonus = (int)(defense * 0.4);
            defense += bonus;
            tempDefenseBonus += bonus;
            isDefending = true; // για να μετρήσει η άμυνα στο damage υπολογισμό

            WriteColored($"🛡️ {name}'s armor glows before the strike! +{bonus} DEF this turn!", ConsoleColor.Cyan);
            Thread.Sleep(500);
        }

        // ⚔️ Animation
        WriteColored($"{name} charges with honor! ⚔️", ConsoleColor.Yellow);
        Thread.Sleep(400);

        string[] frames = { "⚔️ →", "    ⚔️ →", "        ⚔️ →", "             💥" };
        foreach (var f in frames)
        {
            Console.WriteLine(f);
            Thread.Sleep(180);
        }

        WriteColored($"{name} delivers a mighty slash! ⚔️✨", ConsoleColor.Yellow);
        Thread.Sleep(400);

        // Κάνει τώρα την επίθεση
        base.Attack(target, isPlayer);
    }



}

public class MikeK : Character
{
    public MikeK() : base("Mike K", 120, 18, 10) { }

    public override void Attack(Character target, bool isPlayer = false)
    {
        // === Intro animation ===
        WriteColored($"{name} Tostaki??... 🥪⚙", ConsoleColor.Magenta);
        Thread.Sleep(800);

        string[] frames = { "  🥪 →", "        🥪 →", "              🥪 →", "                     💥" };
        foreach (var f in frames)
        {
            Console.WriteLine(f);
            Thread.Sleep(200);
        }

        WriteColored($"{name} throws a flying toast with ham & cheese! 🥪🔥", ConsoleColor.Magenta);
        Thread.Sleep(500);

        // === Apply normal attack ===
        base.Attack(target, isPlayer);

        // === 35% chance to dizzy ===
        if (rand.NextDouble() < 0.33 && target.IsAlive())
        {
            // === Screen shaking / dizzy animation ===
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("\n\n\n\n\n");
                Thread.Sleep(500);
                Console.Clear();
                Thread.Sleep(150);
            }

            WriteColored("🌀 Michail spins the battlefield! 🌀", ConsoleColor.Yellow);

            // === Apply dizzy only if target is not defending ===
            if (!target.isDefending)
            {
                target.skipTurn = true;
                WriteColored($"💫 {target.name} is dizzy and will skip their next turn!", ConsoleColor.Cyan);
            }
            else
            {
                WriteColored($"🛡️ {target.name} blocks the dizzy effect!", ConsoleColor.Blue);
            }
        }
    }
}



// =====================
// === GAME SYSTEM ===
// =====================
public class Game
{
    public void Battle(Character player, Character enemy)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Random rand = new Random();

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n══════════ MEGA BONK BATTLE ══════════\n");
        Console.ResetColor();

        int turn = 1;
        while (player.IsAlive() && enemy.IsAlive())
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n────── TURN {turn} ──────\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green; Console.Write($"{player.name} (YOU): "); player.PrintHPBar();
            Console.ForegroundColor = ConsoleColor.Red; Console.Write($"{enemy.name} (CPU): "); enemy.PrintHPBar();
            Console.ResetColor();

            // Reset temp defense buffs
            if (player.isDefending) { player.defense -= player.tempDefenseBonus; player.tempDefenseBonus = 0; player.isDefending = false; }
            if (enemy.isDefending) { enemy.defense -= enemy.tempDefenseBonus; enemy.tempDefenseBonus = 0; enemy.isDefending = false; }

            // --- Player Action ---
            bool playerAttack = false, playerDefend = false;
            if (player.skipTurn) { WriteColored("💫 You are dizzy and skip your turn!", ConsoleColor.Blue); player.skipTurn = false; }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("👉 Choose action: ");
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("(A)ttack ");
                Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("(D)efend");
                Console.ResetColor();

                Console.Write("> ");
                string choice = Console.ReadLine()?.ToUpper();
                if (choice == "A") playerAttack = true;
                else if (choice == "D") playerDefend = true;
                else WriteColored("❌ Invalid input — you miss your turn!", ConsoleColor.DarkRed);
            }

            // --- Enemy Action ---
            bool enemyAttack = false, enemyDefend = false;
            int aiChoice = rand.Next(0, 2);
            if (enemy.skipTurn) { WriteColored($"💫 {enemy.name} is dizzy and skips turn!", ConsoleColor.Blue); enemy.skipTurn = false; }
            else { enemyAttack = aiChoice == 1; enemyDefend = aiChoice == 0; }

            Thread.Sleep(500);

            // --- Apply Defense first ---
            if (playerDefend) player.Defend(true);
            if (enemyDefend) enemy.Defend(false);

            Thread.Sleep(500);

            // --- Apply Attacks ---
            if (playerAttack) player.Attack(enemy, true);
            if (enemyAttack) enemy.Attack(player, false);

            turn++;
            Thread.Sleep(1000);
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n════════════════════════════════════════\n");
        if (player.IsAlive()) WriteColored($"🎉 YOU WIN! {player.name} defeated {enemy.name}!", ConsoleColor.Green);
        else WriteColored($"💀 YOU LOSE! {enemy.name} was too strong!", ConsoleColor.Red);
        Console.ResetColor();
    }

    private void WriteColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}

// =====================
// === MAIN ENTRY ===
// =====================
public class Program
{
    public static void Main()
    {
        Console.Title = "🦴 MegaBonk Battle Arena 🦴";
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("🦴 Welcome to MegaBonk Battle Arena! 🦴\n");
        Console.ResetColor();

        Console.WriteLine("Choose your character:");
        Console.WriteLine("1️⃣  Calcium\n2️⃣  Robinette\n3️⃣  CL4NK\n4️⃣  Megachad\n5️⃣  Sir Oofie\n6️⃣  Mike K\n");
        Console.Write("Enter number → ");
        string choice = Console.ReadLine();
        Character player = choice switch
        {
            "1" => new Calcium(),
            "2" => new Robinette(),
            "3" => new CL4NK(),
            "4" => new Megachad(),
            "5" => new SirOofie(),
            "6" => new MikeK(),
            _ => new Calcium()
        };

        Random rand = new Random();
        int enemyChoice = rand.Next(1, 7);
        Character enemy = enemyChoice switch
        {
            1 => new Calcium(),
            2 => new Robinette(),
            3 => new CL4NK(),
            4 => new Megachad(),
            5 => new SirOofie(),
            6 => new MikeK(),
            _ => new CL4NK()
        };

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n💀 Your opponent is: {enemy.name} 💀\n");
        Console.ResetColor();

        Game game = new Game();
        game.Battle(player, enemy);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n💬 Thanks for playing MegaBonk Battle Arena!");
        Console.Title = "Created by Dimos Ioannis 2025 (Tezas)";
        Console.ResetColor();
    }
}
