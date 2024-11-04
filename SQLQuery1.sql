/****** Script per comando SelectTopNRows da SSMS  ******/
SELECT IDUtente, count(c.IDCollaboratore)
FROM [DevTreeplat].[dbo].[AGZ_tbl_Collaboratore] c
inner join [dbo].[CLI_rel_ClienteCollaboratore] cc on cc.IDCollaboratore = c.IDCollaboratore
where c.Stato = 1
group by c.IDUtente
having count(c.IDCollaboratore) = 2

select * from [dbo].[AGZ_tbl_Collaboratore]
where idutente = 9123

select * from UOP_tbl_Privilegio

select * from [dbo].[UOP_rel_CollaboratorePrivilegio]
where IDCollaboratore in (11255,11256)


select * from [dbo].[CLI_rel_ClienteCollaboratore]
where IDCollaboratore in (11255,11256)