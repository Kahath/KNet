DROP TABLE IF EXISTS `command`;

CREATE TABLE `command` (
  `name` varchar(50) COLLATE utf8_bin NOT NULL,
  `commandlevel` smallint(5) unsigned NOT NULL DEFAULT '0',
  `description` text COLLATE utf8_bin NOT NULL,
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

/*Data for the table `command` */

insert  into `command`(`name`,`commandlevel`,`description`) values ('cls',65535,'Usage: Clears console'),('command list',65535,'Lists all available commands'),('help',65535,'Usage: returns description or subcommands for wanted command');
