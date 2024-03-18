# FGNetworking
15 Points, Aiming for a VG.

![BoatsShooterNetworkGif](https://github.com/JerrysIRL/FGNetworking/assets/113015090/bc108705-044b-45e9-a861-f43206b8e29f)
-------------------------------
# Assignments
## Overhead Names (1p)

Relevant scripts: 
- [OverHeadName.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Player/OverheadName.cs)
- [Name.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Player/Name.cs)

What I did:
- I get userName from `SaveClientInformationManager` using clientID
- I also create `OverheadName.cs` which displays the name over the player.

## Health Packs (1p)

Relevant scripts: 

- [BasePickup.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Pickups/BasePickup.cs)
- [HealthPickup.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Pickups/HealthPickup.cs)

What I did:
- Created `BasePickup.cs` for future pickups to minimized repetetive code.
- Works similary to a mine except it adds health.

## Sprite Renderer (1p)

Relevant scripts: 

- [PlayerController.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Player/PlayerController.cs)
What I did:
- I created a `NetworkVariable<bool> IsMoving` which is writable by the owner, because this feature has no gameplay impact. I set it true based on RigidBodies velocity.
- Change sprite based on the bools value

## Ammo Packs (1p)

Relevant scripts: 

- [AmmoPickup.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Pickups/AmmoPickup.cs)

What I did:
- Inherits from BasePickup.cs
- Replenishes ammo on collision.

## Limited Ammo (1p)

Relevant scripts: 

- [AmmoManager.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Player/AmmoManager.cs)
- [FiringAction.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Player/FiringAction.cs)

What I did:
- I created a AmmoManager with `NetworkVariable<int> ammoAmount`. 
- Added an extra condition in order to fire a bullet `ammoManager.ammoAmount.Value > 0`

## Shot Timer (1p)

Relevant scripts: 
- [Timer.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Common/Timer.cs)
- [TimerBehaviour.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Common/TimerBehaviour.cs)

What I did:
- I added the cooldown to the Homing Missile, you can fire one every 5 seconds.
- I created a raw C# class `Timer.cs` and networkBehaviour `TimerBehaviour.cs` for modularity.
- TimerBehaviour has netVar<bool> which changes when the duration reaches 0.

## Shield Power-Up (2p)

Relevant scripts: 

- [Shield.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/PowerUp/Shield.cs)
- [Health.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Player/Health.cs#L30-L44)

What I did:
-  Created a script `Shield.cs` which is added on Player on respawn. `NetworkVariable<int> HitPoints`. which keeps track of shields health.
-  Added a UI element to represent if a Player is protected by Shield.

## Limited Respawn (2p)

Relevant scripts: 

- [RespawnManager.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Player/RespawnManager.cs)

What I did:
- Created `RespawnManager.cs` which controlls respawns using NetVar.
- Added invulnerability IEnumerator upon revival.
  
## Player Death (1p)

Relevant scripts: 

- [RespawnManager.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Player/RespawnManager.cs)

What I did:
- When Player loses all respawnPoints I call NetworkManger.Shutdown() and send player to startingScene.

## Homing Missile (3p)

Relevant scripts: 

- [HomingMissile.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Projectiles/HomingMissile.cs)
- [FiringAction.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Player/FiringAction.cs#L26-L35)

What I did:
- Missile is owned by Server and Instantiated using ServerRpc
- I get all connected clients from NetworkManager to find the closest Enemy.
- Missile than chases the player until it hits or the Player destroys it using projectiles.

## Burst of Speed Power-Up (1p)

Relevant scripts: 

- [PlayerController.cs](https://github.com/JerrysIRL/FGNetworking/blob/main/Assets/Scripts/Player/PlayerController.cs#L112-L126)

What I did:
- I created a coroutine which double the speed of the player of 0.5 seconds. Ability has a 2 second cooldown.
- Called using ServerRpc

## Usage
Use the following controls to play the game:
| Controls | Explanation |
| ------ | ------ |
| W and S |  Moving Up and Down | 
| A and D |  Rotate Left and Right | 
| Space Bar |  Launch Missile | 
| L Shift |  Dash / Boost | 
| LMB |  Fire Projectile | 
