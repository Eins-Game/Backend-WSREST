CREATE TABLE unlockedAchievements 
(
	player_id INT PRIMARY KEY NOT NULL REFERENCES players (id),
	achievement_id INT NOT NULL REFERENCES achievements (id),
	progress INT NOT NULL DEFAULT (0)
)
