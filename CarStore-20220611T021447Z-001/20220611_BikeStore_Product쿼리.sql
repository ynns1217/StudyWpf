-- 제품 전체 내용 출력용
SELECT p.product_id as 제품번호
     , p.product_name as 제품명
	 , b.brand_id as 브랜드번호
	 , b.brand_name as 브랜드명
	 , c.category_id as 구분번호
     , c.category_name as 구분명
	 , p.model_year as 출시년도
	 , CONCAT('$ ', p.list_price) as 가격
  FROM production.products as p
 INNER JOIN production.brands as b
    ON p.brand_id = b.brand_id
 INNER JOIN production.categories as c
    ON p.category_id = c.category_id;

