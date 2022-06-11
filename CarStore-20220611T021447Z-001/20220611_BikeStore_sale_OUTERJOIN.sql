--�Ǹ����� OUTER JOIN

SELECT so.[store_id]
     , so.[store_name]
     , so.[phone]
     --, so.[email]
     --, so.[street]
     --, so.[city]
     --, so.[state]
     --, so.[zip_code]
	 , st.[staff_id]
     , st.[first_name] + ' ' + st.[last_name] as full_name
     , st.[email]
     , st.[phone]
     ---, st.[active]
     --, st.[store_id]
     --, st.[manager_id]
	 , od.[order_id]
     , od.[customer_id]
     , CASE WHEN od.[order_status] = 3 THEN '����غ�'
	        WHEN od.[order_status] = 4 THEN '��۽���'
	   END as [��ۻ���]
     , od.[order_date]
     , od.[required_date]
     , od.[shipped_date]
     --, od.[store_id]
     --, od.[staff_id]
	 , cu.[customer_id]
     , cu.[first_name]
     , cu.[last_name]
     , cu.[phone]
     , cu.[email]
     , cu.[street]
     , cu.[city]
     , cu.[state]
     , cu.[zip_code]
  FROM [sales].[stores] as so
 INNER JOIN [sales].[staffs] as st
    ON so.store_id = st.store_id
 INNER JOIN [sales].[orders] as od
    ON so.store_id = od.store_id
   AND st.staff_id = od.staff_id
 INNER JOIN [sales].[customers] as cu
    ON cu.customer_id = od.customer_id;

-- od.order_status = 3 ����غ���
-- od.order_status = 4 ��۽���

