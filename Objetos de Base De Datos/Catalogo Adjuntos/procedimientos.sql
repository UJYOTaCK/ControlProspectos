if exists (select '' from sysobjects  where id = object_id(N'[Catprospectosadjunto_Save]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Catprospectosadjunto_Save]
GO

create proc Catprospectosadjunto_Save
		@ProspectoadjuntoId int OUTPUT,
		@ProspectoId int,
		@TipodocumentoId int,
		@Tipodocumento Varchar(250),
		@PathFile Varchar(100),
		@Estatus int,
		@FechaCreacion datetime,
		@FileUpload image,
		@Ultimaact int OUTPUT
as
begin
declare @updateType varchar(100)

if exists (select '' from CAT_PROSPECTOS_ADJUNTOS (Nolock) where PROSPECTOADJUNTO_ID=@ProspectoadjuntoId) begin
	-- Existe el registro y se actualizará...
	set @updateType='actualizar'

	if cast((select UltimaAct from CAT_PROSPECTOS_ADJUNTOS (nolock) where PROSPECTOADJUNTO_ID=@ProspectoadjuntoId) as int) <> @UltimaAct begin
		raiserror('[MSG #5001]. El registro fué actualizado desde otra locación, no se realizó ninguna actualización.', 16, 1)
		return
	end

	update CAT_PROSPECTOS_ADJUNTOS set 
		PROSPECTO_ID = @ProspectoId,
		TIPODOCUMENTO_ID = @TipodocumentoId,
		TIPODOCUMENTO = @Tipodocumento,
		PATH_FILE = @PathFile,
		ESTATUS = @Estatus,
		FECHA_CREACION = @FechaCreacion,
		FILE_UPLOAD = @FileUpload
	where PROSPECTOADJUNTO_ID=@ProspectoadjuntoId

end else begin
	-- Es un registro nuevo...
	set @updateType='insertar'

	set @ProspectoadjuntoId=(SELECT ISNULL(MAX(PROSPECTOADJUNTO_ID),0)+ 1 FROM CAT_PROSPECTOS_ADJUNTOS (NOLOCK))
	
	insert CAT_PROSPECTOS_ADJUNTOS (
		PROSPECTOADJUNTO_ID, 
		PROSPECTO_ID, 
		TIPODOCUMENTO_ID, 
		TIPODOCUMENTO, 
		PATH_FILE, 
		ESTATUS, 
		FECHA_CREACION, 
		FILE_UPLOAD
		)
	values (
		@ProspectoadjuntoId,
		@ProspectoId,
		@TipodocumentoId,
		@Tipodocumento,
		@PathFile,
		@Estatus,
		@FechaCreacion,
		@FileUpload
		)
	
	end
set @UltimaAct = cast((select ultimaAct from CAT_PROSPECTOS_ADJUNTOS (nolock) where PROSPECTOADJUNTO_ID=@ProspectoadjuntoId) as int)

if @@ERROR <> 0 begin
	raiserror('Error al %s en la tabla CAT_PROSPECTOS_ADJUNTOS', 16, 1, @updateType)
	return
end
end 
GO 


GO 


if exists (select * from sysobjects where id = object_id(N'[Catprospectosadjunto_Select]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Catprospectosadjunto_Select]
GO

create proc Catprospectosadjunto_Select 
@PROSPECTOID int
as
begin
	select 
		PROSPECTOADJUNTO_ID,
		PROSPECTO_ID,
		TIPODOCUMENTO_ID,
		TIPODOCUMENTO,
		PATH_FILE,
		ESTATUS,
		FECHA_CREACION,
		FILE_UPLOAD,
		CAST( UltimaAct as INT) as UltimaAct
	from CAT_PROSPECTOS_ADJUNTOS (nolock)
	where PROSPECTO_ID=@PROSPECTOID
end 
GO 


GO
if exists (select * from sysobjects where id = object_id(N'[Catprospectosadjunto_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Catprospectosadjunto_Delete]
GO
create proc Catprospectosadjunto_Delete
@PROSPECTOID int
as
begin
	DELETE
	from CAT_PROSPECTOS_ADJUNTOS
	where PROSPECTO_ID=@PROSPECTOID
end 
GO 