-- ��ǰ ��ü ���� ��¿�
SELECT p.product_id as ��ǰ��ȣ
     , p.product_name as ��ǰ��
	 , b.brand_id as �귣���ȣ
	 , b.brand_name as �귣���
	 , c.category_id as ���й�ȣ
     , c.category_name as ���и�
	 , p.model_year as ��ó⵵
	 , CONCAT('$ ', p.list_price) as ����
  FROM production.products as p
 INNER JOIN production.brands as b
    ON p.brand_id = b.brand_id
 INNER JOIN production.categories as c
    ON p.category_id = c.category_id;

