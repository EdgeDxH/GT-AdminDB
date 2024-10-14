CREATE TABLE "MIEMBRO" (
	"id"	INTEGER UNIQUE,
	"nombre"	TEXT,
	"fecha_ingreso"	TEXT,
	"ultimo_login"	TEXT,
	PRIMARY KEY("id" AUTOINCREMENT)
);
CREATE TABLE "RAID" (
	"id"	INTEGER UNIQUE,
	"fecha_inicio"	TEXT,
	PRIMARY KEY("id" AUTOINCREMENT)
);
CREATE TABLE "PARTICIPACION" (
	"id"	INTEGER UNIQUE,
	"total_damage"	INTEGER,
	"intentos_totales"	INTEGER,
	"fk_id_raid"	INTEGER,
	"fk_id_miembro"	INTEGER,
	PRIMARY KEY("id" AUTOINCREMENT),
	CONSTRAINT "FK_ID_MIEMBRO" FOREIGN KEY("fk_id_miembro") REFERENCES "MIEMBRO"("id"),
	CONSTRAINT "FK_ID_RAID" FOREIGN KEY("fk_id_raid") REFERENCES "RAID"("id")
);