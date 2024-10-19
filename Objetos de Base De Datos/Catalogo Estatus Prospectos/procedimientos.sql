if exists (select '' from sysobjects  where id = object_id(N'[Catestatusprospecto_Save]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Catestatusprospecto_Save]
GO

create proc Catestatusprospecto_Save
		@EstatusId int OUTPUT,
		@Estatus Varchar(250) OUTPUT,
		@Ultimaact int OUTPUT
as
begin
declare @updateType varchar(100)

if exists (select '' from CAT_ESTATUS_PROSPECTOS (Nolock) where ESTATUS_ID=@EstatusId) begin
	-- Existe el registro y se actualizará...
	set @updateType='actualizar'

	if cast((select UltimaAct from CAT_ESTATUS_PROSPECTOS (nolock) where ESTATUS_ID=@EstatusId) as int) <> @UltimaAct begin
		raiserror('[MSG #5001]. El registro fué actualizado desde otra locación, no se realizó ninguna actualización.', 16, 1)
		return
	end

	update CAT_ESTATUS_PROSPECTOS set 
		ESTATUS = @Estatus
	where ESTATUS_ID=@EstatusId

end else begin
	-- Es un registro nuevo...
	set @updateType='insertar'

	set @EstatusId=(SELECT ISNULL(MAX(ESTATUS_ID),0)+ 1 FROM CAT_ESTATUS_PROSPECTOS (NOLOCK))
	
	insert CAT_ESTATUS_PROSPECTOS (
		ESTATUS_ID, 
		ESTATUS
		)
	values (
		@EstatusId,
		@Estatus
		)
	
	end
set @UltimaAct = cast((select ultimaAct from CAT_ESTATUS_PROSPECTOS (nolock) where ESTATUS_ID=@EstatusId) as int)

if @@ERROR <> 0 begin
	raiserror('Error al %s en la tabla CAT_ESTATUS_PROSPECTOS', 16, 1, @updateType)
	return
end
end 
GO 


GO 


if exists (select * from sysobjects where id = object_id(N'[Catestatusprospecto_Select]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [Catestatusprospecto_Select]
GO

create proc Catestatusprospecto_Select 
as
begin
	select 
		ESTATUS_ID,
		ESTATUS,
		CAST( UltimaAct as INT) as UltimaAct
	from CAT_ESTATUS_PROSPECTOS (nolock)
end 
GO 