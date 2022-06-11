SELECT s.[store_cd]
      ,s.[store_addr]
	  , o.[order_no]
      ,o.[mem_no]
      ,o.[order_date]
      ,o.[store_cd]
  FROM [dbo].[Car_order] as o
 right OUTER JOIN [dbo].[Car_store] as s
    ON s.store_cd = o.store_cd;


