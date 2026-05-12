-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- guia.dbo.Arcanos definition

-- Drop table

-- DROP TABLE guia.dbo.Arcanos;

CREATE TABLE guia.dbo.Arcanos (
	Id int IDENTITY(1,1) NOT NULL,
	Numero int NOT NULL,
	Nombre nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	LetraHebrea nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	SignificadoEsoterico nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Mensaje nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ElementoOPlaneta nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ImagenUrl nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Arcanos PRIMARY KEY (Id)
);


-- guia.dbo.Arquetipos definition

-- Drop table

-- DROP TABLE guia.dbo.Arquetipos;

CREATE TABLE guia.dbo.Arquetipos (
	Id int IDENTITY(1,1) NOT NULL,
	Numero int NOT NULL,
	Nombre nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	LetraHebrea nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	SignificadoEsoterico nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Mensaje nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ElementoOPlaneta nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ImagenUrl nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Arquetipos PRIMARY KEY (Id)
);


-- guia.dbo.ElementosAstro definition

-- Drop table

-- DROP TABLE guia.dbo.ElementosAstro;

CREATE TABLE guia.dbo.ElementosAstro (
	Id int IDENTITY(1,1) NOT NULL,
	Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Icono nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Esencia nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Descripcion nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_ElementosAstro PRIMARY KEY (Id)
);


-- guia.dbo.FasesLunares definition

-- Drop table

-- DROP TABLE guia.dbo.FasesLunares;

CREATE TABLE guia.dbo.FasesLunares (
	Id int IDENTITY(1,1) NOT NULL,
	Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Icono nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	SignificadoEspiritual nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Recomendacion nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_FasesLunares PRIMARY KEY (Id)
);


-- guia.dbo.FrasesGratitud definition

-- Drop table

-- DROP TABLE guia.dbo.FrasesGratitud;

CREATE TABLE guia.dbo.FrasesGratitud (
	Id int IDENTITY(1,1) NOT NULL,
	Texto nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Categoria nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_FrasesGratitud PRIMARY KEY (Id)
);


-- guia.dbo.Personas definition

-- Drop table

-- DROP TABLE guia.dbo.Personas;

CREATE TABLE guia.dbo.Personas (
	Id int IDENTITY(1,1) NOT NULL,
	Nombres nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Apellidos nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	FechaNacimiento datetime2 NOT NULL,
	Email nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	FechaRegistro datetime2 NOT NULL,
	Username nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Personas PRIMARY KEY (Id)
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_Personas_Email ON guia.dbo.Personas (  Email ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE UNIQUE NONCLUSTERED INDEX IX_Personas_Username ON guia.dbo.Personas (  Username ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- guia.dbo.SignosZodiacales definition

-- Drop table

-- DROP TABLE guia.dbo.SignosZodiacales;

CREATE TABLE guia.dbo.SignosZodiacales (
	Id int IDENTITY(1,1) NOT NULL,
	Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Icono nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Elemento nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DescripcionLarga nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	PalabrasClave nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_SignosZodiacales PRIMARY KEY (Id)
);


-- guia.dbo.Temas definition

-- Drop table

-- DROP TABLE guia.dbo.Temas;

CREATE TABLE guia.dbo.Temas (
	Id int IDENTITY(1,1) NOT NULL,
	Titulo nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DescripcionGeneral nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	EstaActivo bit NOT NULL,
	EsGratis bit NOT NULL,
	FechaCreacion datetime2 NOT NULL,
	CONSTRAINT PK_Temas PRIMARY KEY (Id)
);


-- guia.dbo.ArbolesVida definition

-- Drop table

-- DROP TABLE guia.dbo.ArbolesVida;

CREATE TABLE guia.dbo.ArbolesVida (
	Id int IDENTITY(1,1) NOT NULL,
	PersonaId int NOT NULL,
	Kether_Valor int NOT NULL,
	Kether_Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Chokmah_Valor int NOT NULL,
	Chokmah_Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Binah_Valor int NOT NULL,
	Binah_Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Chesed_Valor int NOT NULL,
	Chesed_Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Gevurah_Valor int NOT NULL,
	Gevurah_Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Tiferet_Valor int NOT NULL,
	Tiferet_Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Netzach_Valor int NOT NULL,
	Netzach_Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Hod_Valor int NOT NULL,
	Hod_Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Yesod_Valor int NOT NULL,
	Yesod_Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Malchut_Valor int NOT NULL,
	Malchut_Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Sendero_1_2 int NOT NULL,
	Sendero_1_3 int NOT NULL,
	Sendero_1_6 int NOT NULL,
	Sendero_2_3 int NOT NULL,
	Sendero_2_4 int NOT NULL,
	Sendero_2_6 int NOT NULL,
	Sendero_3_5 int NOT NULL,
	Sendero_3_6 int NOT NULL,
	Sendero_4_5 int NOT NULL,
	Sendero_4_6 int NOT NULL,
	Sendero_4_7 int NOT NULL,
	Sendero_5_6 int NOT NULL,
	Sendero_5_8 int NOT NULL,
	Sendero_6_7 int NOT NULL,
	Sendero_6_8 int NOT NULL,
	Sendero_6_9 int NOT NULL,
	Sendero_7_8 int NOT NULL,
	Sendero_7_9 int NOT NULL,
	Sendero_7_10 int NOT NULL,
	Sendero_8_9 int NOT NULL,
	Sendero_8_10 int NOT NULL,
	Sendero_9_10 int NOT NULL,
	FechaCalculo datetime2 NOT NULL,
	CONSTRAINT PK_ArbolesVida PRIMARY KEY (Id),
	CONSTRAINT FK_ArbolesVida_Personas_PersonaId FOREIGN KEY (PersonaId) REFERENCES guia.dbo.Personas(Id) ON DELETE CASCADE
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_ArbolesVida_PersonaId ON guia.dbo.ArbolesVida (  PersonaId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- guia.dbo.PersonasDetalles definition

-- Drop table

-- DROP TABLE guia.dbo.PersonasDetalles;

CREATE TABLE guia.dbo.PersonasDetalles (
	Id int IDENTITY(1,1) NOT NULL,
	PersonaId int NOT NULL,
	SignoZodiaco nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	FaseLunar nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Elemento nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	VibracionDia nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	MensajeGratitud nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	NotasAstrologicas nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_PersonasDetalles PRIMARY KEY (Id),
	CONSTRAINT FK_PersonasDetalles_Personas_PersonaId FOREIGN KEY (PersonaId) REFERENCES guia.dbo.Personas(Id) ON DELETE CASCADE
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_PersonasDetalles_PersonaId ON guia.dbo.PersonasDetalles (  PersonaId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- guia.dbo.PersonasNumerologia definition

-- Drop table

-- DROP TABLE guia.dbo.PersonasNumerologia;

CREATE TABLE guia.dbo.PersonasNumerologia (
	Id int IDENTITY(1,1) NOT NULL,
	PersonaId int NOT NULL,
	MisionVida int NOT NULL,
	NumeroAlma int NULL,
	NumeroPersonalidad int NOT NULL,
	NumeroDestino int NOT NULL,
	LeccionVida int NOT NULL,
	CONSTRAINT PK_PersonasNumerologia PRIMARY KEY (Id),
	CONSTRAINT FK_PersonasNumerologia_Personas_PersonaId FOREIGN KEY (PersonaId) REFERENCES guia.dbo.Personas(Id) ON DELETE CASCADE
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_PersonasNumerologia_PersonaId ON guia.dbo.PersonasNumerologia (  PersonaId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- guia.dbo.RetosSemanales definition

-- Drop table

-- DROP TABLE guia.dbo.RetosSemanales;

CREATE TABLE guia.dbo.RetosSemanales (
	Id int IDENTITY(1,1) NOT NULL,
	Titulo nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Descripcion nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Instrucciones nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	NumeroAsociado int NULL,
	TemaId int NOT NULL,
	FechaInicio datetime2 NOT NULL,
	FechaFin datetime2 NULL,
	Activo bit NOT NULL,
	EsGlobal bit NOT NULL,
	Dificultad nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_RetosSemanales PRIMARY KEY (Id),
	CONSTRAINT FK_RetosSemanales_Temas_TemaId FOREIGN KEY (TemaId) REFERENCES guia.dbo.Temas(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_RetosSemanales_TemaId ON guia.dbo.RetosSemanales (  TemaId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- guia.dbo.Significados definition

-- Drop table

-- DROP TABLE guia.dbo.Significados;

CREATE TABLE guia.dbo.Significados (
	Id int IDENTITY(1,1) NOT NULL,
	ValorNumero int NOT NULL,
	Apodo nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Mision nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Reto nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Mantra nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Amuleto nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	MensajeMagico nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	TemaId int NOT NULL,
	CONSTRAINT PK_Significados PRIMARY KEY (Id),
	CONSTRAINT FK_Significados_Temas_TemaId FOREIGN KEY (TemaId) REFERENCES guia.dbo.Temas(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Significados_TemaId ON guia.dbo.Significados (  TemaId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- guia.dbo.Bitacoras definition

-- Drop table

-- DROP TABLE guia.dbo.Bitacoras;

CREATE TABLE guia.dbo.Bitacoras (
	Id int IDENTITY(1,1) NOT NULL,
	PersonaId int NOT NULL,
	Fecha datetime2 NOT NULL,
	Tipo nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Contenido nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ValorSincronico nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	EstadoReto nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	RetoId int NULL,
	CONSTRAINT PK_Bitacoras PRIMARY KEY (Id),
	CONSTRAINT FK_Bitacoras_Personas_PersonaId FOREIGN KEY (PersonaId) REFERENCES guia.dbo.Personas(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Bitacoras_RetosSemanales_RetoId FOREIGN KEY (RetoId) REFERENCES guia.dbo.RetosSemanales(Id)
);
 CREATE NONCLUSTERED INDEX IX_Bitacoras_PersonaId ON guia.dbo.Bitacoras (  PersonaId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Bitacoras_RetoId ON guia.dbo.Bitacoras (  RetoId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;