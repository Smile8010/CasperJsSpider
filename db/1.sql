SELECT Name,COUNT(1) AS 'Times'
FROM Product
where RefCatalogLink='https://www.amazon.co.jp/gp/top-sellers/kitchen/ref=crw_ratp_ts_kitchen'
and ranknumber = 1
group by Name

select *
from product
where Asin='B07DL38N1N'

sELECT count(1) from tb_sys_product

select * from tb_sys_catalog where updatestring !='20180730'

select * from tb_sys_catalog where ID='{5817603E-3F35-453D-AE85-FDD72483A1EC}'

SELECT COUNT(1) FROM tb_sys_catalog WHERE updateString != '20180731'

select * from tb_sys_catalog where CatalogProductTableName='MapE6682136460545B88568D9C391079111'