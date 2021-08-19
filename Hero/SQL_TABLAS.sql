
Create database Hero;
Use Hero;

Create table Cliente
(
	Nombres varchar(100),
	Apellidos varchar(100),
	Tipo varchar(100),
	Cedula_Nit varchar(10),
	Correo varchar(200),
	Telefono varchar(15),
	Activo bit, 
	Id int identity(1, 1) primary key
);

Create table Cuenta
(
	Numero varchar(15),
	Tipo varchar(10),
	Saldo float,
	Activo bit,

	ClienteId int foreign key references Cliente(Id),
	Id int identity(1, 1) primary key
);

Create table Movimientos
(
	Fecha datetime,
	Tipo varchar(15),
	Valor float,
	Activo bit,

	CuentaOrigen int foreign key references Cuenta(Id),
	CuentaDestino int foreign key references Cuenta(Id),
	Id int identity(1, 1) primary key 
);

Create table Usuario
(
	Usuario varchar(15) primary key,
	Contraseña varchar(4),
	Activo bit,

	ClienteId int foreign key references Cliente(Id)
)


