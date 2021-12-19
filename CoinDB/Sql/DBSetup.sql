CREATE SCHEMA coins;

CREATE TABLE `coins`.`transaction` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NOT NULL,
  `ticker` VARCHAR(255) NOT NULL,
  `buy_date` DATE NOT NULL,
  `quantity` DOUBLE NOT NULL,
  `price` DOUBLE NOT NULL,
  `total` DOUBLE NOT NULL,
  `staked` TINYINT NOT NULL DEFAULT 0,
  `sold` TINYINT NOT NULL DEFAULT 0,
  `sell_date` DATE NULL,
  PRIMARY KEY (`id`));
  
ALTER TABLE `coins`.`transaction` 
ADD COLUMN `profit` DOUBLE NULL AFTER `sell_date`;

INSERT INTO `coins`.`transaction` (`name`, `ticker`, `buy_date`, `quantity`, `price`, `total`, `staked`, `sold`) VALUES ('monero', 'xmr', '2021-11-18', '1', '226.29', '226.29', '0', '0');