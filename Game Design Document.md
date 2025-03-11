**Project Mutatio**  
by  
Tres Ex Machina

**Pitch:**  
A birds eye view game inspired by binding of isaac. User starts off in a room that has doors leading to doors, some rooms will have mobs you have to fight (those doors lead to the minibosses), players will have a simple pickup/use and attack keybind, arrow keys or WASD to move around the map. If not too complex, will implement a procedural generation algorithm for unique maps, otherwise will use make a preset map (single play type). Eventually the user will get to the boss, but by then will have items like stronger armor or a stronger attack to be able to fight this boss. \[break down this initial pitch into different parts of the document\]

**Setting:**  
The game takes place in a lab with mutations running amok with multiple rooms to explore in the lab before getting to the boss. The player's character is a successful lab experiment themselves that didn’t get mutated in the process and is trying to escape the lab. The bosses and mobs are the characters' mutated siblings. 

**Game Components:**  
	**Objects:**

* Player character  
* GUI: Player HP  
* GUI: Boss/Mob HP  
* GUI: Player update menu  
* Chests/Items/Pedestals  
* Doors/Next Room  
* Enemy  
* Enemy shots  
* Walls  
* Heals \-\> Hearts/syringes/etc…

	**Attributes:**

* Position for player  
* Player Damage/Health Stats  
* Player Direction / Attacking direction

	**Relationships:**

* Press arrow keys or WASD to move up/down/left/right  
  * \[Something about adapting it for the adaptive controllers\]  
* When a \[ARROW\] key is pressed, the player attacks  
* When the \[E\] key is pressed, the player picks up the item they’re standing over  
* (is there a hotbar? Usable items? Maybe press a num to select an item on the hotbar, and press the currently selected item’s number to use it)  
* When an enemy’s attack makes contact with the player’s hitbox, the player loses health  
* When the player’s attack makes contact with an enemy’s hitbox, the enemy loses health  
* The GUI: When the player’s health changes 

**Game Mechanics:**  
	In Project Mutatio, the player is initially placed in an empty room. The player controls the character with WASD to move and arrows keys to attack in the currently pressed arrow key direction. The player has fixed speed/attack damage/attack speed/health until they find items to boost or even decrease said stat. The player will decide which direction to go in based on the available doors opened. Once they cross the door, the next room generates and enemies and traps will appear.   
	The goal of the game is to clear each room of the enemies and get to the final boss while acquiring upgrades for your stats and killing the boss without dying. 

**Optional features:**  
	High Score list  
	Multiple Bosses  
	Multiple Items to boost attack/speed/health  
	

**Team Members:**  
Kenneth Chau: No Unity experience, 4 years of coding, experience with graphic arts and VFX  
Sergio Haro: Some Unity experience (Did part of a Udemy course a year ago), 3 years of coding, pixel art  
Anh Hoang: No Unity experience, 4 years of coding (2 in C\#), digital art, UI/UX

**Division of Labor:**  
	Kenneth Chau: TBD  
	Sergio Haro: TBD  
Anh Hoang: TBD  
 