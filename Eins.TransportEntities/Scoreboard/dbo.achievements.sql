CREATE TABLE achievements 
(
	id INT PRIMARY KEY UNIQUE NOT NULL,
	name VARCHAR NOT NULL,
	description VARCHAR NULL,
	target_number INT NOT NULL DEFAULT (- 1)
)
