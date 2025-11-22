select * from PETICIONES

/*
DECLARE @StartDate AS DATE = '2023-01-01';
DECLARE @EndDate AS DATE = '2023-12-31';

SELECT DATEADD(DAY, RAND(CHECKSUM(NEWID())) * (1 + DATEDIFF(DAY, @StartDate, @EndDate)), @StartDate) AS RandomDate;

	start: '2017-12-01',
	end: '2018-02-02',
	completed: {
		amount: 0.95
	},
	name: 'Prototyping'
*/


--json_query(QUOTENAME(STRING_AGG('"' + STRING_ESCAPE(item_id, 'json') + '"', char(44)))) as [json]
select * from PETICIONES

select FORMAT(FECHA_PETICION,'yyyy-MM-dd') 'start', FORMAT(FECHA_RESP_PETICION,'yyyy-MM-dd') 'end', completed.amount 'completed.amount', SUBSTRING(PETICIONES.DESC_PETICION, 0, 15) 'name'
from PETICIONES 
inner join (select SUBSTRING(CAST(RAND() as nvarchar), 0, 5) amount ) completed on completed.amount is not null
where FECHA_PETICION is not null and FECHA_RESP_PETICION is not null
for json path

select JSON_QUERY(N'{"name": "Four"}')
from PETICIONES


select SUBSTRING(CAST(RAND() as nvarchar), 0, 5)
