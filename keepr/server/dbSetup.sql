CREATE TABLE IF NOT EXISTS accounts (
    id VARCHAR(255) NOT NULL PRIMARY KEY COMMENT 'primary key',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
    name VARCHAR(255) COMMENT 'User Name',
    email VARCHAR(255) UNIQUE COMMENT 'User Email',
    picture VARCHAR(255) COMMENT 'User Picture'
) default charset utf8mb4 COMMENT '';

CREATE TABLE keeps (
    id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'primary key',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
    name VARCHAR(255) COMMENT 'Keep Name',
    description TEXT COMMENT 'Keep Description',
    img VARCHAR(1000) COMMENT 'Keep Image',
    views INT DEFAULT 0 COMMENT 'Number of Views',
    kept INT DEFAULT 0 COMMENT 'Number of Keeps',
    creator_id VARCHAR(255) NOT NULL COMMENT 'Owner Id',
    FOREIGN KEY (creator_id) REFERENCES accounts (id) ON DELETE CASCADE
);

CREATE TABLE vaults (
    id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'primary key',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
    name VARCHAR(255) NOT NULL COMMENT 'Vault Name',
    description VARCHAR(1000) NOT NULL COMMENT 'Vault Description',
    img VARCHAR(1000) NOT NULL COMMENT 'Vault Image',
    is_private BOOLEAN DEFAULT FALSE COMMENT 'Is Private',
    creator_id VARCHAR(255) NOT NULL COMMENT 'Owner Id',
    FOREIGN KEY (creator_id) REFERENCES accounts (id) ON DELETE CASCADE
);

CREATE TABLE vault_keeps (
    id INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'primary key',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
    vault_id INT NOT NULL COMMENT 'Vault Id',
    keep_id INT NOT NULL COMMENT 'Keep Id',
    creator_id VARCHAR(255) NOT NULL COMMENT 'Owner Id',
    FOREIGN KEY (vault_id) REFERENCES vaults (id) ON DELETE CASCADE,
    FOREIGN KEY (keep_id) REFERENCES keeps (id) ON DELETE CASCADE,
    FOREIGN KEY (creator_id) REFERENCES accounts (id) ON DELETE CASCADE
);

/*REVIEW Hopefully I don't need this for vault_keeps UNIQUE (vault_id, keep_id) */

/* Keeps TABLE section */

ALTER TABLE keeps MODIFY COLUMN name VARCHAR(255) NOT NULL;

ALTER TABLE keeps
MODIFY COLUMN description TEXT NOT NULL COMMENT 'Keep Description',
MODIFY COLUMN img VARCHAR(1000) NOT NULL COMMENT 'Keep Image',
MODIFY COLUMN views INT NOT NULL DEFAULT 0 COMMENT 'Number of Views',
MODIFY COLUMN kept INT NOT NULL DEFAULT 0 COMMENT 'Number of Keeps';

SELECT keeps.*, accounts.*
From keeps
    INNER JOIN accounts ON accounts.id = keeps.creator_id
WHERE
    keeps.creator_id = accounts.id;

/* Vaults TABLE section */

SELECT vaults.*, accounts.*
FROM vaults
    JOIN accounts ON vaults.creator_id = accounts.id
WHERE
    vaults.id = 1;

SELECT vaults.*, accounts.*
FROM vaults
    JOIN accounts ON vaults.creator_id = accounts.id;

/* VaultKeeps TABLE section */
SELECT vault_keeps.*, vaults.*, keeps.*, accounts.*
FROM
    vault_keeps
    JOIN vaults ON vault_keeps.vault_id = vaults.id
    JOIN keeps ON vault_keeps.keep_id = keeps.id
    JOIN accounts ON vault_keeps.creator_id = accounts.id

SELECT vault_keeps.*, keeps.*, accounts.*
FROM
    vault_keeps
    JOIN keeps ON vault_keeps.keep_id = keeps.id
    JOIN accounts ON keeps.creator_id = accounts.id
WHERE
    vault_keeps.vault_id = 15;

/* Accounts TABLE section */

SELECT vaults.*
FROM vaults
WHERE
    accountId = "687afdc8522de155ec38cc42";

SELECT vaults.*
FROM vaults
WHERE
    creator_id = "687afdc8522de155ec38cc42";

ALTER TABLE accounts
ADD COLUMN cover_img VARCHAR(1000) COMMENT "Cover Image" AFTER picture;

SELECT accounts.*
FROM accounts
WHERE
    id = "687afdc8522de155ec38cc42";

SELECT vaults.*, accounts.*
FROM vaults
    JOIN accounts ON vaults.creator_id = accounts.id
WHERE
    accounts.id = "687afdc8522de155ec38cc42";

SELECT vault_keeps.* FROM vault_keeps

SELECT keeps.*, accounts.*, count(vk.id) AS kept
FROM
    keeps
    JOIN accounts ON keeps.creator_id = accounts.id
    LEFT JOIN vault_keeps vk ON keeps.id = vk.keep_id
WHERE
    keeps.id = 21
GROUP BY keeps.id;


SELECT
        vault_keeps.*,
        keeps.*,
        accounts.*,
        count(vk.id) AS KeepsInVault
        FROM vault_keeps
        JOIN keeps ON vault_keeps.keep_id = keeps.id
        JOIN accounts ON keeps.creator_id = accounts.id
        LEFT JOIN vault_keeps vk ON keeps.id = vk.keep_id
        WHERE vault_keeps.vault_id = 146
        GROUP BY vault_keeps.id;


SELECT vault_keeps.*
        FROM vault_keeps
        WHERE id = 50;