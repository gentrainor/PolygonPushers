# PolygonPushers

[Jira Board Link](https://polygonpushers.atlassian.net/jira/software/projects/SCRUM/boards/1?atlOrigin=eyJpIjoiMTMwYjE1NWE2MGExNDcwNzljYmIxMTE3NzdhOTVmNTMiLCJwIjoiaiJ9&sprintStarted=true)


Start Scene:
Start Menu.unity 

WASD - move,
space - jump,
shift - run,

red stars to collect around map, physics based obstacles left choice, other obstacles on right, goal block at end.
timer & score on ui,
pause menu - esc
ai enemy on course

contributions:

john pham - imported character and connected mecanim animations, some playercontroller.cs (refined movement as well), pausemenu.cs, levelcomplete.cs, some scorecounter.cs and sfx. Added visual indicator to enemyAI.cs, as well as modified enemy detection & attack

Seong Won - Implemented the Enemy AI system using NavMesh configured waypoints, and integrated it across all scenes (created EnemyAI.cs, AI_Enemy.prefab, Waypoints objects). Fixed some errors. Created GoalBox_Mat material with emission, implemented GoalVisualEffect.cs for rotation/hover/pulse effects, and applied the new visual system to the LevelGoal object. Replaced the red cube with a new 3D star model by updating the scene's object setup and adding the custom mesh file (star_2_5D_thick.obj). Also modified the existing star object configuration inside the scene to use the new mesh.

Genevieve Trainor - created start menu, fixed camera position, created timer when game starts, respawn feature when character touches ground he respawns. Also did some game feel and created first three obstacles and the wall enclosure and starting platform. startmenu.cs, racetimer.cs, startzone.cs and edited some playercontroller.cs. Edit LevelCompleteCollision.cs to start level 2. Added MoveUpDown.cs to balls in level 2. Updated AI configuration and Waypoint placements. Edited ResetOnGround.cs: score decreases when player respawns. 

Mena Doce - Created BouncePad.cs and HorizontalMover.cs - Created hoop obstacle on start of leve1 1 as well as hill and hoop obstacles on level 1. On level two I created the moving tile obstacle where I used HorizontalMover.cs and I created the bounce obstacle where I utilized Bouncepad.cs - I also added distinction for harder levels with red path.

David Gabriel - Created ScoreCounter.cs and ScoreText object to implement the score counter and the two obstacles at the start of each of the paths. Created animation Push Obstacle.anim and Move Cylinder.anim animations, and Push Obstacle.controller and Cylinder.controller to create the pushing obstacles on the the left side of level 1 and the 3 obstacle of level 2. Implemented CursorScript.cs to hide the cursor during gameplay. Created gear and ball obstacles on the left side of level 2 and implemented RotateZ.cs to move the gear obstacles.
