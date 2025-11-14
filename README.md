# PolygonPushers

[Jira Board Link](https://polygonpushers.atlassian.net/jira/software/projects/SCRUM/boards/1?atlOrigin=eyJpIjoiMTMwYjE1NWE2MGExNDcwNzljYmIxMTE3NzdhOTVmNTMiLCJwIjoiaiJ9&sprintStarted=true)


Start Scene:
Start Menu.unity 

WASD - move,
space - jump,
shift - run,

red dots to collect around map, physics based obstacles left choice, other obstacles on right, goal block at end.
timer & score on ui,
pause menu - esc
ai enemy on course

movement is somewhat "slippery"

contributions:

john pham - imported character and connected mecanim animations, some playercontroller.cs, pausemenu.cs, levelcomplete.cs, some scorecounter.cs and sfx. 

Seong Won - implemented the Enemy AI system using NavMesh configured waypoints, and integrated it across all scenes. Fixed some errors.

Genevieve Trainor - created start menu, fixed camera position, created timer when game starts, respawn feature when character touches ground he respawns. Also did some game feel and created first three obstacles and the wall enclosure and starting platform. startmenu.cs, racetimer.cs, startzone.cs and edited some playercontroller.cs. Edit LevelCompleteCollision.cs to start level 2. Added MoveUpDown.cs to balls in level 2. 

Mena Doce - Created obstacles on both paths of the scene, including hill obstacle and obstacles where player has to go through loops to reach end

David Gabriel - Created the score counter and the two obstacles at the start of each of the paths.
