if exists (select '' from sysobjects  where id = object_id(N'[Cattiposdocumento_Save]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Cattiposdocumento_Save]
GO

create proc Cattiposdocumento_Save
		@TipodocumentoId int OUTPUT,
		@Tipodocumento Varchar(250) OUTPUT,
		@Ultimaact int OUTPUT
as
begin
declare @updateType varchar(100)

if exists (select '' from CAT_TIPOS_DOCUMENTOS (Nolock) where TIPODOCUMENTO_ID=@TipodocumentoId) begin
	-- Existe el registro y se actualizará...
	set @updateType='actualizar'

	if cast((select UltimaAct from CAT_TIPOS_DOCUMENTOS (nolock) where TIPODOCUMENTO_ID=@TipodocumentoId) as int) <> @UltimaAct begin
		raiserror('[MSG #5001]. El registro fué actualizado desde otra locación, no se realizó ninguna actualización.', 16, 1)
		return
	end

	update CAT_TIPOS_DOCUMENTOS set 
		TIPODOCUMENTO = @Tipodocumento
	where TIPODOCUMENTO_ID=@TipodocumentoId

end else begin
	-- Es un registro nuevo...
	set @updateType='insertar'

	set @TipodocumentoId=(SELECT ISNULL(MAX(TIPODOCUMENTO_ID),0)+ 1 FROM CAT_TIPOS_DOCUMENTOS (NOLOCK))
	
	insert CAT_TIPOS_DOCUMENTOS (
		TIPODOCUMENTO_ID, 
		TIPODOCUMENTO
		)
	values (
		@TipodocumentoId,
		@Tipodocumento
		)
	
	end
set @UltimaAct = cast((select ultimaAct from CAT_TIPOS_DOCUMENTOS (nolock) where TIPODOCUMENTO_ID=@TipodocumentoId) as int)

if @@ERROR <> 0 begin
	raiserror('Error al %s en la tabla CAT_TIPOS_DOCUMENTOS', 16, 1, @updateType)
	return
end
end 
GO 


GO 


if exists (select * from sysobjects where id = object_id(N'[Cattiposdocumento_Select]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Cattiposdocumento_Select]
GO

create proc Cattiposdocumento_Select
as
begin
	select 
		TIPODOCUMENTO_ID,
		TIPODOCUMENTO,
		CAST( UltimaAct as INT) as UltimaAct
	from CAT_TIPOS_DOCUMENTOS (nolock)
end 
GO 