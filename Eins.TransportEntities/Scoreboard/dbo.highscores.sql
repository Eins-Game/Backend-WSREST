CREATE TABLE highscores 
(
player_id INT PRIMARY KEY UNIQUE NOT NULL REFERENCES players (id),
games_played INT DEFAULT (0) NOT NULL, games_won INT NOT NULL DEFAULT (0),
achievements_unlocked INT NOT NULL DEFAULT (0)
)
