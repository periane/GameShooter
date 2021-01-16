### GameShooter
#### Examen de CSharp
Réalisation d'un jeu sous Unity 3D: Game Shooter

###UML Simplfié :

### ntp_player
 - Speed: float = 10f
 - drag: float = 1
 - thrust: float
 - projectile: GameObject : 4f
 - projectileSpeed: float = 4f
 - Start(): void
 - Update(): void
 - fireRate: float = .25f
 - nextFire: float
 - ntp_FixedUpdate(): void
 - ntp_Move(): void
 - ntp_Fire(): void
 - ntp_Shoot(): void
 - ntp_FixedUpdate
 
### ntp_ennemis
 - Speed: float = 5f
 - vector2 speed
 - points: int = 10
 - projectile: GameObject
 - projectileSpeed: float = 4f
 - fireRate: float = .25f
 - nextFire: float 
 - GameManager: gameManager
 - Start(): void
 - Update(): void
 - OnTriggerEnter2D(): void
 - ntp_Move(): void
 - ntp_Fire(): void
 - ntp_Shoot(): void
 
### ntp_gameManager
 - state : States
 - level, score, lives: int
 - levelTxt, scoreTxt, livesTxt: Text, messageTxt: Text
 - player, enemi,boom: GameObject
 - waitToStart, networkPanel: GameObject
 - cam: Camera
 - height, width: float
 - Start(): void
 - Update(): void
 - ntp_LaunchGame(): void
 - ntp_InitGame(): void
 - ntp_LoadWaves(): void
 - ntp_UpdateTexts(): void
 - ntp_AddScore: void
 - ntp_EndOfWaves(): void
 - ntp_NextWaves(): IEnumerator
 - ntp_KillPlayer(): void
 - ntp_PlayerAgain(): IEnumerator
 - ntp_GameOver(): void

### ntp_bullet
 - Update(): void
 - life: float = 3f

### ntp_pauseManager
 - gameIsPaused: bool = false
 - Start(): void
 - Update(): void
 - ntp_PauseGame(): void
 
 ### waves (Enemi)

