# **Project Mutatio**  
by Tres Ex Machina

### **Pitch:**  
Project Mutatio is a bird’s-eye view game in which players control an experimental test subject attempting to escape the laboratory they were created in. The player must explore the lab, facing aggressive, failed experiments as well as dangerous environmental traps. By the last room, they will have acquired stat-enhancing mutations and/or equipment, which will improve their ability to defeat the final boss and earn their freedom.

### **Setting:**  
The game takes place in a lab with mutated specimens running amok with multiple rooms to explore in the lab before getting to the boss. The player's character is a successful, healthy lab experiment and is trying to escape the lab. The bosses and mobs are the characters' deformed, mutated “siblings.”

### **Game Components:**  
#### **Objects:**

	* Player character  
	* GUI: Player HP  
	* GUI: Boss/Mob HP  
	* GUI: Player status menu  
	* Minimap  
	* Chests/Items/Pedestals  
	* Doors/Next Room  
	* Enemy  
	* Enemy shots  
	* Walls  
	* Healing/buff items (Hearts/syringes/etc…)

#### **Attributes:**

	* Position for player  
	* Player damage/health/speed stats  
	* Player direction/attacking direction  
	* Enemy damage/health/speed stats  
	* Completion time  
	* Pressed status for buttons  
	* Items in player inventory

#### **Relationships:**

	* Press WASD or arrow keys (on keyboard or adaptive controller’s D-pad) to move up/down/left/right  
	* When an \[ARROW\] key is pressed, the player attacks  
	* When the \[E\] key is pressed, the player picks up the item they’re standing over.  
	  * If the player’s inventory is full, the currently selected item and the item the player intends to pick up will be swapped.  
	* When a number key from 1 to 5 is pressed, the corresponding item on the player’s hotbar inventory is selected.   
	  * When \[X\] key is pressed, the player uses the item.  
	* When an enemy’s attack makes contact with the player’s hitbox, the player loses health.  
	* When the player’s attack makes contact with an enemy’s hitbox, the enemy loses health.  
	* The GUI: When a player or enemy’s health changes, their health bar flashes and updates to reflect the new value.  
	* The GUI: When the player completes the game, the final screen shows the length of their run (not counting pauses).  
	* The GUI: When the player encounters a new room, the layout of the laboratory is updated on the minimap.  
	* The GUI: When the player moves between rooms, the minimap reflects their new location.

### **Game Mechanics:**  
In Project Mutatio, the player is initially placed in an empty room. The player controls the character with WASD to move and arrows keys to attack in the currently pressed arrow key direction. The player has fixed speed/attack damage/attack speed/health until they find items to boost or even decrease said stat. The player will decide which direction to go in based on the available doors opened. Once they cross the door, the next room generates and enemies and traps will appear. If not too complex, a procedural generation algorithm will be used to create unique maps; otherwise, there will be a preset map. Eventually the user will get to the final boss, but by then will have items like stronger armor or a stronger attack to be able to fight this boss.  
The goal of the game is to clear each room of the enemies and get to the final boss while acquiring upgrades for your stats and killing the boss without dying. 

### **Optional features:**  
	* High Score list  
	* Multiple Bosses  
	* Multiple Items to boost attack/speed/health  
	

## **Team Members:**  
Kenneth Chau: No Unity experience, 4 years of coding, experience with graphic arts and VFX  
Sergio Haro: Some Unity experience (Did part of a Udemy course a year ago), 3 years of coding, pixel art  
Anh Hoang: No Unity experience, 4 years of coding (2 in C\#), digital art, UI/UX

## **Division of Labor**
<!-- using html because github markdown doesn't support multi-column tables :( 
	strike out completed stuff like
	<td><s>this</s><td> -->
<table>
  <tr>
    <th colspan="3">Game Design</th>
  </tr>
  <tr>
    <td>Control scheme</td>
    <td>Stat balance</td>
    <td>Player mechanics</td>
  </tr>
  <tr>
    <td>Boss/enemy mechanics</td>
    <td>Buff/debuff effects</td>
    <td>Tutorial design</td>
  </tr>
	
  <tr>
    <th colspan="3">Programming</th>
  </tr>
  <tr>
    <td>Player movement</td>
    <td>Player attacks</td>
    <td>Enemy pathfinding</td>
  </tr>
  <tr>
    <td>Enemy attacks</td>
    <td>Health/damage system</td>
    <td>Item pickup</td>
  </tr>
  <tr>
    <td>Item usage</td>
    <td>Item hotbar</td>
    <td>Trap behavior</td>
  </tr>
  <tr>
    <td><i>Procedural room generation</i></td>
    <td><i>High-score tracking</i></td>
    <td><i>Minimap room tracking</i></td>
  </tr>
	
  <tr>
    <th colspan="3">VFX</th>
  </tr>
  <tr>
    <td>Concept art</td>
    <td>Player character design</td>
    <td>Enemy design</td>
  </tr>
  <tr>
    <td>Attack design</td>
    <td>Environment design</td>
    <td>Player sprites</td>
  </tr>
  <tr>
    <td>Enemy sprites</td>
    <td>Item icons</td>
    <td>Tilemaps</td>
  </tr>
  <tr>
    <td>GUI (HP bars, menus)</td>
    <td>Trap design</td>
    <td><i>Minimap design</i></td>
  </tr>
  
  <tr>
    <th colspan="3">SFX</th>
  </tr>
  <tr>
    <td>Music</td>
    <td>Attack/damage SFX</td>
    <td>GUI SFX</td>
  </tr>
</table>
