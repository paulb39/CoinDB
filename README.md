# CoinDB

Install:
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
