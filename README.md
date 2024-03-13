
# FGNetworking
13 Points right now, Aiming for VG.

-------------------------------
# Assignments
## Overhead Names (1p)

Relevant commits: 
- [3546b38](https://github.com/JerrysIRL/FGNetworking/commit/3546b3892db9110003db4f6e483741501e3c5628)
- [d2e2d96](https://github.com/JerrysIRL/FGNetworking/commit/d2e2d96ef5f34e1da49ee74ba3393693de92fae5)

What I did:
- I get userName from `SaveClientInformationManager` using clientID
- I also create `OverheadName.cs` which displays the name over the player.

## Health Packs (1p)

Relevant commits: 

- [d2e2d96](https://github.com/JerrysIRL/FGNetworking/commit/d2e2d96ef5f34e1da49ee74ba3393693de92fae5)
- [b2f8db0](https://github.com/JerrysIRL/FGNetworking/commit/b2f8db0065576356a09d4f53ebcd1655713000ab)

What I did:
- Created `BasePickup.cs` for future pickups to minimized repetetive code.
- Works similary to a mine except it adds health.

## Sprite Renderer (1p)

Relevant commits: 

- [4ed2f4d](https://github.com/JerrysIRL/FGNetworking/commit/4ed2f4dd29057d910be52221b970859a9e184dae)

What I did:
- I created a `NetworkVariable<bool> IsMoving` which is writable by the owner, because this feature has no gameplay impact. I set it true based on RigidBodies velocity.
- Change sprite based on the bools value

## Ammo Packs (1p)

Relevant commits: 

- [00f52de](https://github.com/JerrysIRL/FGNetworking/commit/00f52de0d91c985bc60506ec9eacca0b4df0afab)

What I did:
- Inherits from BasePickup.cs
- Replenishes ammo on collision.

## Limited Ammo (1p)

Relevant commits: 

- [00f52de](https://github.com/JerrysIRL/FGNetworking/commit/00f52de0d91c985bc60506ec9eacca0b4df0afab)
- [54b5733](https://github.com/JerrysIRL/FGNetworking/commit/54b573398880ea372b9c2d30aad655edcd8e8f39#diff-69bcbbf001a381f979af0cb1522b6665470a52ca87545fb778f77bc992858a8e)

What I did:
- I created a AmmoManager with `NetworkVariable<int> ammoAmount`. 
- Added an extra condition in order to fire a bullet `ammoManager.ammoAmount.Value > 0`

## Shot Timer (1p)

Relevant commits: 
- [1c556aa](https://github.com/JerrysIRL/FGNetworking/commit/1c556aa5635dc88eae5ec0216b36bcf2ffd64a09)

What I did:
- I created a raw C# class `Timer.cs` and networkBehaviour `TimerBehaviour.cs` for modularity.
- TimerBehaviour has netVar<bool> which changes when the duration reaches 0.

## Shield Power-Up (2p)

Relevant commits: 

- [bbabe0a](https://github.com/JerrysIRL/FGNetworking/commit/bbabe0a6ba262b4fdcaec33cc9288e00949de5fe) 
- [6cbef89](https://github.com/JerrysIRL/FGNetworking/commit/6cbef89d8091222a135afeb9758ffd82fcf3b720)
- [5bcdc63](https://github.com/JerrysIRL/FGNetworking/commit/5bcdc63b2d47b02e082b6292e2c71f05fcd3262a) 

What I did:
-  Created a script `Shield.cs` which is added on Player on respawn. `NetworkVariable<int> HitPoints`. which keeps track of shields health.
-  Added a UI element to represent if a Player is protected by Shield.

## Limited Respawn (2p)

Relevant commits: 

- [73043ce](https://github.com/JerrysIRL/FGNetworking/commit/73043cea019ef15bec66535482b9ab6862fdf91e#diff-c6f6a0e090cc0b65e2a6249e4b75ff575e123868b9b7d73a0d3367bd3297ca83)

What I did:
- Created `RespawnManager.cs` which controlls respawns using NetVar.
- Added invulnerability IEnumerator upon revival.
  
## Player Death (1p)

Relevant commits: 

- [013f9fe](https://github.com/JerrysIRL/FGNetworking/commit/013f9febef655deba17fcfb598a3035ee4cc21c1)

What I did:
- When Player loses all respawnPoints I call NetworkManger.Shutdown() and send player to startingScene.

## Homing Missile (3p)

Relevant commits: 

- [dd73930](https://github.com/JerrysIRL/FGNetworking/commit/dd739301a36cb0eacb56cec839cf92cb93228d86)
- [de1c1c5](https://github.com/JerrysIRL/FGNetworking/commit/de1c1c5c747b56f2c2482a9f351c5dd6eee0333b)

What I did:
- Missile is owned by Server and Instantiated using ServerRpc
- I get all connected clients from NetworkManager to find the closest Enemy.
- Missile than chases the player until it hits or the Player destroys it using projectiles.

## Burst of Speed Power-Up (1p)

Relevant commits: 

- [067c39d](https://github.com/JerrysIRL/FGNetworking/commit/067c39de7eb3822b92f95934959f7b14da6a64ef#diff-8a33bf7d07e4ebf5ca72056c234569e18cdd0db9fd8466228009e190813f0d97)

What I did:
- I created a coroutine which double the speed of the player of 0.5 seconds. Ability has a 2 second cooldown.
- Called using ServerRpc

## Usage
Use the following controls to play the game:
| Controls | Explanation |
| ------ | ------ |
| W and S |  moving up and down | 
| A and D |  rotate left and right | 
| SpaceBar |  Launch Missile | 
| LMB |  Fire Projectile | 
