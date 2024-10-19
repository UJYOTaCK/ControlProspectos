if exists (select '' from sysobjects  where id = object_id(N'[Catprospecto_Save]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Catprospecto_Save]
GO

create proc Catprospecto_Save
		@ProspectoId int,
		@Nombre Varchar(100),
		@ApellidoPaterno Varchar(100),
		@ApellidoMaterno Varchar(100),
		@Calle Varchar(250),
		@Numero Varchar(100),
		@Colonia Varchar(100),
		@CodigoPostal Varchar(10),
		@Telefono Varchar(20),
		@Rfc Varchar(20),
		@Estatus int,
		@Observaciones Varchar(max),
		@Ultimaact int,
		@ProspectoIdR int OUTPUT,
		@UltimaactR int OUTPUT
as
begin



declare @updateType varchar(100)

if exists (select '' from CAT_PROSPECTOS (Nolock) where PROSPECTO_ID=@ProspectoId) begin
	-- Existe el registro y se actualizará...
	set @updateType='actualizar'

	if cast((select UltimaAct from CAT_PROSPECTOS (nolock) where PROSPECTO_ID=@ProspectoId) as int) <> @UltimaAct begin
		raiserror('[MSG #5001]. El registro fué actualizado desde otra locación, no se realizó ninguna actualización.', 16, 1)
		return
	end

	update CAT_PROSPECTOS set 
		NOMBRE = @Nombre,
		APELLIDO_PATERNO = @ApellidoPaterno,
		APELLIDO_MATERNO = @ApellidoMaterno,
		CALLE = @Calle,
		NUMERO = @Numero,
		COLONIA = @Colonia,
		CODIGO_POSTAL = @CodigoPostal,
		TELEFONO = @Telefono,
		RFC = @Rfc,
		ESTATUS = @Estatus,
		OBSERVACIONES = @Observaciones
	where PROSPECTO_ID=@ProspectoId

end else begin
	-- Es un registro nuevo...
	set @updateType='insertar'

	set @ProspectoId=(SELECT ISNULL(MAX(PROSPECTO_ID),0)+ 1 FROM CAT_PROSPECTOS (NOLOCK))
	
	insert CAT_PROSPECTOS (
		PROSPECTO_ID, 
		NOMBRE, 
		APELLIDO_PATERNO, 
		APELLIDO_MATERNO, 
		CALLE, 
		NUMERO, 
		COLONIA, 
		CODIGO_POSTAL, 
		TELEFONO, 
		RFC, 
		ESTATUS, 
		OBSERVACIONES
		)
	values (
		@ProspectoId,
		@Nombre,
		@ApellidoPaterno,
		@ApellidoMaterno,
		@Calle,
		@Numero,
		@Colonia,
		@CodigoPostal,
		@Telefono,
		@Rfc,
		@Estatus,
		@Observaciones
		)
	
	end


set @UltimaAct = cast((select ultimaAct from CAT_PROSPECTOS (nolock) where PROSPECTO_ID=@ProspectoId) as int)

set @ProspectoIdR = @ProspectoId;
set @UltimaActR = @UltimaAct;


if @@ERROR <> 0 begin
	raiserror('Error al %s en la tabla CAT_PROSPECTOS', 16, 1, @updateType)
	return
end
end 
GO 


GO 


if exists (select * from sysobjects where id = object_id(N'[Catprospecto_Select]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Catprospecto_Select]
GO

create proc Catprospecto_Select 
@ProspectoId int = null,
@estatus int = null
as
begin

	if @ProspectoId <= 0 begin
		set @ProspectoId = null
	end

	if @estatus <= 0 begin
		set @estatus = null
	end

	select 
		PROSPECTO_ID,
		NOMBRE,
		APELLIDO_PATERNO,
		APELLIDO_MATERNO,
		CALLE,
		NUMERO,
		COLONIA,
		CODIGO_POSTAL,
		TELEFONO,
		RFC,
		ESTATUS,
		OBSERVACIONES,
		CAST( UltimaAct as INT) as UltimaAct
	from CAT_PROSPECTOS (nolock)
	where PROSPECTO_ID=coalesce(@ProspectoId,PROSPECTO_ID)
	and ESTATUS = coalesce(@estatus,ESTATUS)
end 
GO 

